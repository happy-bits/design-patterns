/*
 ABSTRACT FACTORY (own example)
 */
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace DesignPatterns.AbstractFactory
{
    [TestClass]
    public class Cards
    {
        static readonly List<string> _log = new List<string>();

        [TestMethod]
        public void Ex1()
        {
            new Client().Main();

            CollectionAssert.AreEqual(new[] {

                "Shuffling cards",
                "You have 2 cards on your hand",
                "The deck have 50 cards",

                "Blandar korten",
                "Din hand består av 2 kort",
                "Leken har 50 kort",

            }, _log);

        }


        interface IAbstractFactory
        {
            AbstractDeck CreateDeck();
            AbstractPlayer CreatePlayer();
        }

        class SwedishCardFactory : IAbstractFactory
        {
            public AbstractDeck CreateDeck() => new SwedishDeck();
            public AbstractPlayer CreatePlayer() => new SwedishPlayer();
        }

        class EnglishCardFactory : IAbstractFactory
        {
            public AbstractDeck CreateDeck() => new EnglishDeck();
            public AbstractPlayer CreatePlayer() => new EnglishPlayer();
        }

        abstract class AbstractDeck
        {
            protected readonly Queue<Card> _cards;
            public AbstractDeck()
            {
                _cards = new Queue<Card>();

                _cards.Enqueue(new Card(8, Color.Diamonds));

                for (int i = 0; i < 51; i++)
                {
                    _cards.Enqueue(new Card(1, Color.Spades));
                }
            }
            public virtual void Shuffle()
            {
                // norwegian shuffle
            }

            internal Card PickTopCard()
            {
                return _cards.Dequeue();
            }

            public abstract void Describe();
        }

        class SwedishDeck : AbstractDeck
        {
            public override void Describe()
            {
                _log.Add($"Leken har {_cards.Count} kort");
            }

            public override void Shuffle()
            {
                _log.Add("Blandar korten");
                base.Shuffle();
            }
        }

        class EnglishDeck : AbstractDeck
        {
            public override void Describe()
            {
                _log.Add($"The deck have {_cards.Count} cards");
            }

            public override void Shuffle()
            {
                _log.Add("Shuffling cards");
                base.Shuffle();
            }
        }

        abstract class AbstractPlayer
        {
            protected Queue<Card> _hand = new Queue<Card>();

            public void PickOneCardFrom(AbstractDeck deck)
            {
                Card card = deck.PickTopCard();
                _hand.Enqueue(card);
            }

            public abstract void DescribeHand();

            public IEnumerable<Card> Hand => _hand;
        }

        class SwedishPlayer : AbstractPlayer
        {
            public override void DescribeHand() 
            {
                _log.Add($"Din hand består av {_hand.Count} kort");
            }
        }

        class EnglishPlayer : AbstractPlayer
        {
            public override void DescribeHand()
            {
                _log.Add($"You have {_hand.Count} cards on your hand");
            }
        }

        class Card
        {

            public Card(int rank, Color diamonds)
            {
                if (rank < 1 || rank > 13)
                    throw new ArgumentException();

                Rank = rank;
                Diamonds = diamonds;
            }

            public int Rank { get; }
            public Color Diamonds { get; }
        }

        enum Color
        {
            Spades, Hearts, Clubs, Diamonds
        }


        class Client
        {
            public void Main()
            {
                ClientMethod(new EnglishCardFactory());
                ClientMethod(new SwedishCardFactory());
            }

            void ClientMethod(IAbstractFactory factory)
            {
                var player = factory.CreatePlayer();
                var deck = factory.CreateDeck();

                deck.Shuffle();
                player.PickOneCardFrom(deck);
                player.PickOneCardFrom(deck);

                player.DescribeHand();
                deck.Describe();
            }
        }
    }
}
