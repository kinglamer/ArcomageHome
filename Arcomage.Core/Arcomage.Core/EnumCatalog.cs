using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arcomage.Core
{
    public enum GameEvent
    {
        None,
        Used,
        Droped
    }

    public enum SelectPlayer
    {
        First = 0,
        Second,
        None
    }

    public enum CurrentAction
    {
        None, //Произошел первый запуск
        StartGame, //Начало игры
        GetPlayerCard, //Получить карты для игрока
        WaitHumanMove, //Ожидание хода игрока
        HumanUseCard, //Игрок использовал карту
        HumanCanPlayAgain, //Игрок может сходить еще раз
        AnimateHumanMove, //Анимация использования карты игрока
        UpdateStatHuman, //Обновление статистики игрока
        UpdateStatAI, //Обновление статистики компьютера
        EndHumanMove, //Завершение хода игрока, берутся еще карты
        PlayerMustDropCard, //Статус, что игрок обязан сбросить карту
        PlayAgain, //Флаг того, что нужно сыграть еще одну карту
        AIUseCard, //Флаг того, что компьютер завершил использование всех карт (появилось в следствие того, что есть карты, которые заставляют брать еще карту)
        AIMoveIsAnimated, //Анимация стола противника
        AIUseCardAnimation, //Анимация использование хода противника
        EndAIMove, //Завершение хода противника
        EndGame, //Завершение игры
        PassStroke, //пропуск хода
        GetAICard //компьютер берет следующую добавочную карту
    }


}
