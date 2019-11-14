using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LandLord.Core.Room
{
    public class GameRoom : GameRoomMetaData
    {
        public static GameRoom Create(Guid Id)
        {
            var room = new GameRoom()
            {
                Id = Id,
            };
            // initialize the _cards with 3 emtpy cards list
            room.Cards.Add(new List<PlayerCard>());
            room.Cards.Add(new List<PlayerCard>());
            room.Cards.Add(new List<PlayerCard>());
            // intilize the _players with 3 empty player
            room.Players.Add(Player.Empty);
            room.Players.Add(Player.Empty);
            room.Players.Add(Player.Empty);
            return room;
        }

        public static GameRoom Prepare()
        {
            var room = GameRoom.Create(Guid.NewGuid());
            var cards = Facade.CreateFullCards();
            cards = Facade.Shuffle(cards);
            var (reserved, (cards1, cards2, cards3)) = Facade.Deal(cards);
            // change PlayingCard list to IList<PlayerCard>
            IList<PlayerCard> convertCardList(IList<PlayingCard> cards)
            {
                var result = cards.Select(c => (PlayerCard)c).ToList();
                return result;
            }

            room.Cards[0] = convertCardList(cards1);
            room.Cards[1] = convertCardList(cards2);
            room.Cards[2] = convertCardList(cards3);
            room.ReservedCards = new List<PlayingCard>(reserved);
            return room;
        }

        public static GameRoom FromMetaData(IGameRoomMetaData data)
        {
            var room = GameRoom.Create(data.Id);
            room.LandLordIndex = data.LandLordIndex;
            room.CurrentTurn = data.CurrentTurn;
            room.Cards = data.Cards;
            room.Players = data.Players;
            room.ReservedCards = data.ReservedCards;
            room.PrevCards = data.PrevCards;
            room.PrevIndex = data.PrevIndex;
            return room;
        }
        public IGameRoomMetaData ExportMetaData() =>
            this as IGameRoomMetaData;

        public bool AddUser(int nth, Player player)
        {
            if (nth >= 0 && nth < 3 && this.Players[nth].IsEmpty)
            {
                this.Players[nth] = player;
                return true;
            }
            else
                return false;
        }

        public bool AddUser(Player player)
        {
            var nth =
                this.Players
                    .Select((p, i) => (p, i))
                    .Where(t => t.p.IsEmpty || t.p.Id == player.Id)// could replace self
                    .Select(item => item.i)
                    .FirstOrDefault();
            return this.AddUser(nth, player);
        }

        public void AppendCards(IList<PlayingCard> cards)
        {
            var originalCards = this.Cards[this.LandLordIndex];
            foreach (var c in cards)
            {
                originalCards.Add(c);
            }
        }
        public void SetLandLord(int nth)
        {
            this.LandLordIndex = nth;
            this.AppendCards(ReservedCards);
            this.CurrentTurn = nth;
        }

        private bool playCards(int nth, IList<PlayingCard> cards)
        {
            var cardss = cards.Select(c => (PlayerCard)c);
            if (nth < 3 && nth >= 0)
            {
                var originalCards = this.Cards[nth];
                var remaining = originalCards.Except(cardss).ToList();
                this.Cards[nth] = remaining;
                this.PrevIndex = nth;
                this.PrevCards = cards;
                this.CurrentTurn = (this.CurrentTurn + 1) % 3;
                return true;
            }
            else
                throw new Exception("the nth must be an int between [0,2]");
        }

        private bool HasStarted() 
        { 
            var turn = this.LandLordIndex;
            if (this.Cards[turn].Count == 20)
                return false;
            // check other conditions?
            return true;
        }

        public bool StartPlayingCards(IList<PlayingCard> cards) {
            var turn = this.LandLordIndex;
            // check whether the game has started
            if (this.HasStarted())
                return false;

            cards = Facade.Sort(cards);
            if(Facade.CanStartPlaying(cards)) 
            {
                this.CurrentTurn = this.LandLordIndex;
                return this.playCards(turn, cards);
            } 

            return false;
        }

        public bool PlayCards(int nth, IList<PlayingCard> cards)
        {
            // pass 
            if (this.PrevIndex == this.CurrentTurn)
                return this.playCards(nth, cards);
            else
            {
                if (Facade.CanPlay(this.PrevCards, cards))
                    return this.playCards(nth, cards);
                else
                    return false;
            }
        }

        public bool PlayCardsEx(int nth, IList<PlayingCard> cards) =>
            (this.HasFinished, this.Players.Count, this.Cards[this.LandLordIndex].Count)
            switch {
                (false, 3, 20) => this.StartPlayingCards(cards),
                (false, 3, var count) when count < 20 => this.PlayCards(nth, cards),
                _ => false,
            };

        public bool PassCards()
        {
            this.CurrentTurn = (this.CurrentTurn + 1) % 3;
            return true;
        }

        public bool HasWin(int nth)
        {
            if (!this.HasStarted())
            {
                return false;
            }
            var cards = this.Cards[nth];
            return cards.Count == 0;
        }

        public PlayerFindings FindPlayer(string userId)
        {
            var kvs =
             this.Players
                .Select((p, nth) => (nth, p))
                .Where(item => item.p.Id == userId);

            var count = kvs.Count();

            if (count == 0)
                return null;
            if (count > 1)
                throw new Exception($"There're too many player that has Id = {userId}");
            else {
                var (i, p) = kvs.First();
                return new PlayerFindings(i, p);
            }
        }

        private static IList<IList<PlayerCard>> ShadowCardsList(IList<IList<PlayerCard>> fromCardsList, int nth )
        {
            if (nth < 0 || nth > 2)
            {
                var msg = $"invalid argument of {nameof(nth)}={nth}";
                throw new ArgumentException(msg);
            }

            var cardsList = new List<IList<PlayerCard>>();
            cardsList.Add(new List<PlayerCard>());
            cardsList.Add(new List<PlayerCard>());
            cardsList.Add(new List<PlayerCard>());

            // sort by weight
            var fromCards = 
                fromCardsList[nth]
                .OrderBy(c => c switch { 
                    PlayingCard card => card.GetWeight(), 
                    Shadowed s=> throw new Exception("shadowed card can not be shadowed again")
                });
            var toCards = cardsList[nth];

            // copy nth player's cards
            foreach (var c in fromCards)
                toCards.Add(c);
            // shadow other's cards
            for(var i = 0; i < 3; i++) {
                if (i == nth) continue;
                cardsList[i] = fromCardsList[i].Select(c => (PlayerCard)new Shadowed()).ToList();
            }
            return cardsList;
        }
        public GameRoom ShadowCopy()
        {
            var room = GameRoom.Create(this.Id);
            room.LandLordIndex = this.LandLordIndex;
            room.CurrentTurn = this.CurrentTurn;
            room.PrevCards = this.PrevCards;
            room.PrevIndex = this.PrevIndex;
            room.Cards = this.Cards;
            room.Players = this.Players;
            room.ReservedCards = this.ReservedCards;
            return room;
        }

        public IGameRoomMetaData ShadowCards(int nth)
        {
            if (nth > 2 || nth < 0)
            {
                var msg = $"invalid {nameof(nth)}={nth}";
                throw new ArgumentException(msg);
            }
            else {
                var metadata = this.ShadowCopy();
                var newCardsList = GameRoom.ShadowCardsList(this.Cards, nth);
                metadata.Cards = newCardsList;
                // we don't change the list of players / reserverdCards
                //    because they won't be changed: this interface has no setter
                return metadata;
            }
        }



    }
}
