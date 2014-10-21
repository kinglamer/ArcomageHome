ArcomageHome
============

_Тестовый проект Unity3d и .NET Серверной части_

Данный проект мы делаем, чтобы научиться разработке. Это некоммерческий проект 

Описание игры
==


Есть три типа генераторов ресурсов


* Генераторы Quarry (каменоломни, ниже для краткости - шахты) производят bricks (кирпичи, камни).
* Генераторы Magic (ниже - магия) производят gems (геммы, жемчужины). 
* Генераторы Zoo (зоопарки, зверинцы) поризводят beasts (зверей). 

Каждый генератор производит одну штуку соответствующего ресурса за ход. Количество генераторов может быть разным, но не менее одного каждого типа.

В центре игрового экрана расположены строения: красная башня игрока, синяя башня противника и защитные стены перед ними.

В игре по 29 красных, синих, зеленых карт. Любая карта имеет цену - за ход этой картой отбирается определенное количество камней (красные карты), гемм (синие карты), зверей (зеленые карты). 

_Можно сбросить карту без игры (правой кнопкой мыши), при этом ресурсы не отбираются, а ход передается противнику._

Ход вызывает одно из трех изменений (у игрока или у противника): 
* изменение количества генераторов
* изменение количества ресурсов
* измененение высоты строений. 

Кроме того, некоторые карты предоставляют право повторного хода или право сброса одной карты без передачи хода.


Условия победы
==

либо довести высоту своей башни до заданного уровня, либо собрать определенное количество ресурса (любого), либо уничтожить башню противника

В зависимости от тактики противник преимущественно использует атакующие или "строительные" карты, либо этот выбор происходит случайно.
