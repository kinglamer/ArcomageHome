﻿using System;
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

            if (CurrentPlayer.Type == TypePlayer.AI)
                CurrentPlayer.gameActions.Add(GameAction.PlayCard);

            EnemyPlayer = _players[enemy];
            SetPlayerCards(CurrentPlayer);
            SetPlayerCards(EnemyPlayer);
        }

        //TODO: возможно придется переделать список gameActions для персонажей
        /// <summary>
        /// На текущий момент данный метод надо вызывать после каждого уведомления CardPicker о том, что произошло
        /// </summary>
        public void Update()
        {
            while (CurrentPlayer.gameActions.Count > 0)
            {
                var card = CurrentPlayer.ChooseCard();

                //если карты нет, а присутствует действией, которое необходимо выполнить, прерывается метод Update до тех пор, пока не придет уведомление из вне о новых картах
                if (card == null && (CurrentPlayer.gameActions.Contains(GameAction.PlayCardAgain) ||
                        CurrentPlayer.gameActions.Contains(GameAction.DropCard) ||
                        CurrentPlayer.gameActions.Contains(GameAction.PlayCard)))
                    break;

                var playerActions = new List<GameAction>(CurrentPlayer.gameActions);

                //Зашил, что если нужно сбросить карту, то сначала происходит сброс, а уже потом другие действия
                if (CurrentPlayer.gameActions.Contains(GameAction.DropCard))
                {
                    playerActions.Clear();
                    playerActions.Add(GameAction.DropCard);
                }

                foreach (var actions in playerActions)
                {
                    switch (actions)
                    {
                        case GameAction.DropCard:
                            DropCard(card);
                            break;
                        case GameAction.PlayCard:
                            PlayerCard(card);
                            break;
                        case GameAction.PlayCardAgain:
                            PlayerCard(card);
                            break;
                        case GameAction.EndMove:
                            NextPlayerTurn();
                            break;
                    }
                }
            }
            

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
        
        /// <summary>
        /// Использование карты игроком
        /// </summary>
        /// <param name="id">Уникальный номер карты в БД</param>
        private void PlayerCard(Card card)
        {
            if (CurrentPlayer.gameActions.Contains(GameAction.EndMove))
                return;

            if (CanUseCard(card.price))
            {
                if (CurrentPlayer.gameActions.Contains(GameAction.PlayCardAgain))
                    CurrentPlayer.gameActions.Remove(GameAction.PlayCardAgain);

                if (CurrentPlayer.gameActions.Contains(GameAction.PlayCard))
                    CurrentPlayer.gameActions.Remove(GameAction.PlayCard);


                if (!_specialCardHandlers.ContainsKey(card.id))
                    card.Apply(CurrentPlayer, EnemyPlayer);
                else
                {
                    _specialCardHandlers[card.id].copyParams(card);
                    _specialCardHandlers[card.id].Apply(CurrentPlayer, EnemyPlayer);

                    if (_specialCardHandlers[card.id].discard)
                        CurrentPlayer.gameActions.Add(GameAction.DropCard);
                }

                if (card.playAgain)
                    CurrentPlayer.gameActions.Add(GameAction.PlayCardAgain);



                _logCard.Add(new GameCardLog(CurrentPlayer, GameAction.PlayCard, card, CurrentMove));
                Debug.Print(CurrentPlayer.PlayerName + " use " + card.id);
                CurrentPlayer.Cards.Remove(card);
            }
      
            if (CurrentPlayer.gameActions.Count == 0)
                CurrentPlayer.gameActions.Add(GameAction.EndMove);
        }

        private void DropCard(Card card)
        {
            if (CurrentPlayer.gameActions.Contains(GameAction.EndMove))
                return;

            if (card.id == 40)
            {
                Log.Info("Эту карту нельзя сбросить!");
                CurrentPlayer.gameActions.Remove(GameAction.DropCard);
                return;
            }

            try
            {
                CurrentPlayer.gameActions.Remove(GameAction.DropCard);
                _logCard.Add(new GameCardLog(CurrentPlayer, GameAction.DropCard, card, CurrentMove));
                Debug.Print(CurrentPlayer.PlayerName + " drop " + card.id);
                CurrentPlayer.Cards.Remove(card);
            }
            catch
            {
                Log.Error(string.Format("Player: {0} can't pass card {1}", CurrentPlayer.PlayerName, card.name));
            }

            if (CurrentPlayer.gameActions.Count == 0)
                CurrentPlayer.gameActions.Add(GameAction.EndMove);
        }


        private void NextPlayerTurn()
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

            //Т.к. вызов этого метода происходит в Update, то ход не прерывается, а передается AI автоматически
            if(CurrentPlayer.Type == TypePlayer.AI)
                CurrentPlayer.gameActions.Add(GameAction.PlayCard);
        }


        //TODO: избавить от методов, которые перечислены ниже
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

        private Card GetCardById(int id, out int index)
        {
            index = CurrentPlayer.Cards.FindIndex(x => x.id == id);
            return CurrentPlayer.Cards[index];
        }
    
    }
}