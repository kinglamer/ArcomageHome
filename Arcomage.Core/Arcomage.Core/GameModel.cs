﻿﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using Arcomage.Core.ArcomageService;
using Arcomage.Core.Common;
using Arcomage.Core.Interfaces;
using Arcomage.Core.SpecialCard;
using Arcomage.Entity;
using Arcomage.Entity.Cards;
﻿using Arcomage.Entity.Players;
﻿using Newtonsoft.Json;


namespace Arcomage.Core
{
    public class GameModel
    {
        public Player CurrentPlayer { get; set; }
        public Player EnemyPlayer { get; set; }
        public bool IsGameEnded { get; set; }
        public string Winner { get; private set; }

        public int CurrentMove;
        private readonly int _maxCard;

        protected readonly ILog Log;
      

        readonly List<Card> _serverCards = new List<Card>();
        private readonly Dictionary<int, Card> _specialCardHandlers;

        /// <summary>
        /// Стэк карт с сервера, чтобы реже обращаться к нему
        /// </summary>
        private readonly Queue<Card> _qCard = new Queue<Card>();

        private readonly List<GameCardLog> _logCard = new List<GameCardLog>();

        private readonly List<Player> _players;


        public GameModel(ILog log, List<Player> players,List<Card> serverCards, Dictionary<int, Card> specialCardHandlers,int currentPlayer, int enemy, int maxCard)
        {
            IsGameEnded = false;
            _maxCard = maxCard;
            _specialCardHandlers = specialCardHandlers;
            Log = log;
            _players = players;
            CurrentMove = 0;
            _serverCards = serverCards;

            CurrentPlayer = _players[currentPlayer];
            EnemyPlayer = _players[enemy];
            SetPlayerCards(CurrentPlayer);
            SetPlayerCards(EnemyPlayer);
        }

        /// <summary>
        /// Устанавливаем карты для игрока
        /// </summary>
        /// <returns></returns>
        private void SetPlayerCards(Player player)
        {
            if (_qCard.Count < _maxCard)
            {
                _serverCards.Randomize(); //TODO: стоил ли перемешивать список карт, каждый раз перед добавлением в стэк?
                foreach (var item in _serverCards)
                {
                    _qCard.Enqueue(item);
                }
            }

            while (player.Cards.Count < _maxCard)
            {
                if (_qCard.Count == 0)
                    break;

                var newCard = _qCard.Dequeue();
                newCard.description = ParseDescription.Parse(newCard.description);
                player.Cards.Add(newCard);
            }
        }

        public List<Card> GetUsedCard(TypePlayer typePlayer, GameAction gameAction)
        {
            return _logCard.Where(x => x.Player.Type == typePlayer && x.GameAction == gameAction).Select(item => item.Card).ToList();
        }

        
        /// <summary>
        /// Проверка хватает ли ресурсов для использования карты
        /// </summary>
        public bool CanUseCard(Price price)
        {
            if (CurrentPlayer.PlayerParams[price.attributes] >= price.value)
                return true;

            return false;
        }

        public bool CanUseCard(int id)
        {
            int index;
            return CanUseCard(GetCardById(id, out index).price);
        }
        

        /// <summary>
        /// Использование карты игроком
        /// </summary>
        /// <param name="id">Уникальный номер карты в БД</param>
        /// <param name="dropCard">Флаг, что карта сбрасывается</param>
        public void MakePlayerMove(int id, bool dropCard = false)
        {
            if (CurrentPlayer.gameActions.Contains(GameAction.EndMove))
                return;

            int index;
            var card = GetCardById(id, out index);

            if (CanUseCard(card.price) && !dropCard)
            {

                if (CurrentPlayer.gameActions.Contains(GameAction.MakeMoveAgain))
                    CurrentPlayer.gameActions.Remove(GameAction.MakeMoveAgain);

                if (!_specialCardHandlers.ContainsKey(id))
                    card.Apply(CurrentPlayer, EnemyPlayer);
                else
                {
                    _specialCardHandlers[id].copyParams(card);
                    _specialCardHandlers[id].Apply(CurrentPlayer, EnemyPlayer);

                    if (_specialCardHandlers[id].discard)
                        CurrentPlayer.gameActions.Add(GameAction.DropCard);
                }

                if (card.playAgain)
                    CurrentPlayer.gameActions.Add(GameAction.MakeMoveAgain);



                _logCard.Add(new GameCardLog(CurrentPlayer, GameAction.MakeMove, CurrentPlayer.Cards[index], CurrentMove));
                Debug.Print(CurrentPlayer.PlayerName + " use " + CurrentPlayer.Cards[index].id);
                CurrentPlayer.Cards.RemoveAt(index);
            }
            else
            {
                if (id == 40 && dropCard)
                {
                    Log.Info("Эту карту нельзя сбросить!");
                    return;
                }

                try
                {
                    CurrentPlayer.gameActions.Remove(GameAction.DropCard);
                    _logCard.Add(new GameCardLog(CurrentPlayer, GameAction.DropCard, CurrentPlayer.Cards[index], CurrentMove));
                    Debug.Print(CurrentPlayer.PlayerName + " drop " + CurrentPlayer.Cards[index].id);
                    CurrentPlayer.Cards.RemoveAt(index);
                }
                catch
                {
                    Log.Error(string.Format("Player: {0} can't pass card {1}", CurrentPlayer.PlayerName, CurrentPlayer.Cards[index].name));
                }
            }

            if (CurrentPlayer.gameActions.Count == 0)
                CurrentPlayer.gameActions.Add(GameAction.EndMove);
        }


        public void NextPlayerTurn()
        {
            if (!CurrentPlayer.gameActions.Contains(GameAction.EndMove) || IsGameEnded)
                return;

            SetPlayerCards(CurrentPlayer);

            CurrentPlayer.UpdateParams();
            CurrentPlayer.gameActions.Clear();

            Winner = GameControllerHelper.CheckPlayerParams(_players);
            if (Winner.Length > 0)
            {
                Log.Info("Победил: " + Winner);
                IsGameEnded = true;
                return;
            }

            Player temp = CurrentPlayer;
            CurrentPlayer = EnemyPlayer;
            EnemyPlayer = temp;
        }

        private Card GetCardById(int id, out int index)
        {
            index = CurrentPlayer.Cards.FindIndex(x => x.id == id);
            return CurrentPlayer.Cards[index];
        }
    
    }
}