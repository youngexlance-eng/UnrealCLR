using System;
using System.Collections.Generic;
using System.Drawing;
using UnrealEngine.Framework;

namespace Game.Casino
{
    /// <summary>
    /// Main Casino Game Manager - Handles all casino game logic and UI
    /// </summary>
    public class CasinoGame
    {
        private static Player currentPlayer;
        private static Random random = new Random();
        private static bool isGameRunning = true;

        public static void OnWorldBegin()
        {
            Debug.AddOnScreenMessage(-1, 15.0f, Color.Gold, "╔════════════════════════════════╗");
            Debug.AddOnScreenMessage(-1, 14.0f, Color.Gold, "║   WELCOME TO UNREAL CASINO   ║");
            Debug.AddOnScreenMessage(-1, 13.0f, Color.Gold, "╚════════════════════════════════╝");
            
            InitializePlayer();
            DisplayMainMenu();
        }

        private static void InitializePlayer()
        {
            currentPlayer = new Player("High Roller", 5000.0);
            currentPlayer.DisplayStats();
        }

        private static void DisplayMainMenu()
        {
            Debug.AddOnScreenMessage(-1, 10.0f, Color.Cyan, "┌─ MAIN MENU ─────────────────────┐");
            Debug.AddOnScreenMessage(-1, 9.5f, Color.Cyan, "│ [1] 🎰 SLOT MACHINE            │");
            Debug.AddOnScreenMessage(-1, 9.0f, Color.Cyan, "│ [2] 🃏 BLACKJACK               │");
            Debug.AddOnScreenMessage(-1, 8.5f, Color.Cyan, "│ [3] 🎲 DICE ROLL               │");
            Debug.AddOnScreenMessage(-1, 8.0f, Color.Cyan, "│ [4] 🪙 COIN FLIP               │");
            Debug.AddOnScreenMessage(-1, 7.5f, Color.Cyan, "│ [5] 💰 ROULETTE                │");
            Debug.AddOnScreenMessage(-1, 7.0f, Color.Cyan, "│ [6] 📊 PLAYER STATS            │");
            Debug.AddOnScreenMessage(-1, 6.5f, Color.Cyan, "│ [0] 🚪 EXIT                    │");
            Debug.AddOnScreenMessage(-1, 6.0f, Color.Cyan, "└──────────────────────────────────┘");
        }

        public static void OnWorldPrePhysicsTick(float deltaTime)
        {
            if (!isGameRunning) return;

            // Display current balance continuously
            Debug.AddOnScreenMessage(100, 10.0f, Color.YellowGreen, 
                $"💵 Balance: ${currentPlayer.GetBalance():F2}");
            
            if (currentPlayer.GetBalance() <= 0)
            {
                Debug.AddOnScreenMessage(-1, 5.0f, Color.Red, 
                    "═════ GAME OVER - OUT OF MONEY ═════");
                isGameRunning = false;
            }
        }
    }

    /// <summary>
    /// Player class - Tracks balance, wins, and game statistics
    /// </summary>
    public class Player
    {
        private string name;
        private double balance;
        private double totalWinnings;
        private int totalGamesPlayed;
        private int totalWins;

        public Player(string playerName, double initialBalance)
        {
            name = playerName;
            balance = initialBalance;
            totalWinnings = 0;
            totalGamesPlayed = 0;
            totalWins = 0;
        }

        public string GetName() => name;
        public double GetBalance() => balance;
        public double GetTotalWinnings() => totalWinnings;
        public int GetGamesPlayed() => totalGamesPlayed;
        public int GetTotalWins() => totalWins;

        public bool PlaceBet(double amount)
        {
            if (amount > 0 && amount <= balance)
            {
                balance -= amount;
                return true;
            }
            return false;
        }

        public void AddWinnings(double amount)
        {
            balance += amount;
            if (amount > 0)
            {
                totalWinnings += amount;
                totalWins++;
            }
        }

        public void IncrementGamesPlayed() => totalGamesPlayed++;

        public void DisplayStats()
        {
            Debug.AddOnScreenMessage(-1, 5.0f, Color.LimeGreen, "╔════════ PLAYER STATS ════════╗");
            Debug.AddOnScreenMessage(-1, 4.5f, Color.LimeGreen, $"║ Name: {name,-24}║");
            Debug.AddOnScreenMessage(-1, 4.0f, Color.LimeGreen, 
                $"║ Balance: ${balance:F2,-20}║");
            Debug.AddOnScreenMessage(-1, 3.5f, Color.LimeGreen, 
                $"║ Total Winnings: ${totalWinnings:F2,-12}║");
            Debug.AddOnScreenMessage(-1, 3.0f, Color.LimeGreen, 
                $"║ Games Played: {totalGamesPlayed,-12}║");
            Debug.AddOnScreenMessage(-1, 2.5f, Color.LimeGreen, 
                $"║ Total Wins: {totalWins,-17}║");
            Debug.AddOnScreenMessage(-1, 2.0f, Color.LimeGreen, "╚═════════════════════════════╝");
        }
    }

    /// <summary>
    /// Stunning Slot Machine Game with visual effects
    /// </summary>
    public class SlotMachine
    {
        private static string[] reels = { "🍒", "🔔", "💎", "⭐", "7️⃣", "👑" };
        private static Random random = new Random();

        public static void PlaySlots(Player player)
        {
            Debug.AddOnScreenMessage(-1, 15.0f, Color.Gold, "╔═══════════════════════════════╗");
            Debug.AddOnScreenMessage(-1, 14.5f, Color.Gold, "║     🎰 SLOT MACHINE 🎰       ║");
            Debug.AddOnScreenMessage(-1, 14.0f, Color.Gold, "║   Match 3 for JACKPOT!       ║");
            Debug.AddOnScreenMessage(-1, 13.5f, Color.Gold, "╚═══════════════════════════════╝");

            double bet = 100;
            
            if (!player.PlaceBet(bet))
            {
                Debug.AddOnScreenMessage(-1, 12.0f, Color.Red, "❌ Insufficient balance!");
                return;
            }

            // Spin animation
            for (int i = 0; i < 3; i++)
            {
                Debug.AddOnScreenMessage(-1, 11.0f + (i * 0.5f), Color.Cyan, "🎯 SPINNING...");
            }

            string reel1 = reels[random.Next(reels.Length)];
            string reel2 = reels[random.Next(reels.Length)];
            string reel3 = reels[random.Next(reels.Length)];

            // Display results with animation
            Debug.AddOnScreenMessage(-1, 10.0f, Color.Yellow, $"┌─────────────────────────────┐");
            Debug.AddOnScreenMessage(-1, 9.5f, Color.Yellow, $"│  {reel1}  {reel2}  {reel3}          │");
            Debug.AddOnScreenMessage(-1, 9.0f, Color.Yellow, $"└─────────────────────────────┘");

            double winnings = 0;

            if (reel1 == reel2 && reel2 == reel3)
            {
                if (reel1 == "7️⃣")
                {
                    winnings = bet * 100;
                    Debug.AddOnScreenMessage(-1, 8.0f, Color.Gold, 
                        "🎊 JACKPOT! JACKPOT! JACKPOT! 🎊");
                    Debug.AddOnScreenMessage(-1, 7.5f, Color.Gold, 
                        $"💰 YOU WIN: ${winnings:F2}");
                }
                else
                {
                    winnings = bet * 10;
                    Debug.AddOnScreenMessage(-1, 8.0f, Color.LimeGreen, 
                        "✨ THREE IN A ROW! ✨");
                    Debug.AddOnScreenMessage(-1, 7.5f, Color.LimeGreen, 
                        $"🏆 YOU WIN: ${winnings:F2}");
                }
            }
            else if (reel1 == reel2 || reel2 == reel3)
            {
                winnings = bet * 3;
                Debug.AddOnScreenMessage(-1, 8.0f, Color.DeepSkyBlue, "⭐ TWO MATCHES! ⭐");
                Debug.AddOnScreenMessage(-1, 7.5f, Color.DeepSkyBlue, 
                    $"💵 YOU WIN: ${winnings:F2}");
            }
            else
            {
                Debug.AddOnScreenMessage(-1, 8.0f, Color.Red, "❌ NO MATCH - YOU LOSE");
                Debug.AddOnScreenMessage(-1, 7.5f, Color.Red, $"Bet Lost: ${bet:F2}");
            }

            if (winnings > 0)
            {
                player.AddWinnings(winnings);
            }

            player.IncrementGamesPlayed();
        }
    }

    /// <summary>
    /// Blackjack Game with dealer AI
    /// </summary>
    public class BlackjackGame
    {
        private static Random random = new Random();

        public static void PlayBlackjack(Player player)
        {
            Debug.AddOnScreenMessage(-1, 15.0f, Color.Gold, "╔═══════════════════════════════╗");
            Debug.AddOnScreenMessage(-1, 14.5f, Color.Gold, "║      🃏 BLACKJACK 🃏         ║");
            Debug.AddOnScreenMessage(-1, 14.0f, Color.Gold, "║   Get 21 or beat dealer!     ║");
            Debug.AddOnScreenMessage(-1, 13.5f, Color.Gold, "╚═══════════════════════════════╝");

            double bet = 150;

            if (!player.PlaceBet(bet))
            {
                Debug.AddOnScreenMessage(-1, 12.0f, Color.Red, "❌ Insufficient balance!");
                return;
            }

            int playerHand = DealCard() + DealCard();
            int dealerHand = DealCard() + DealCard();

            Debug.AddOnScreenMessage(-1, 11.0f, Color.Cyan, 
                $"🎴 Your Hand: {playerHand}");
            Debug.AddOnScreenMessage(-1, 10.5f, Color.Cyan, 
                $"🎴 Dealer Showing: {dealerHand / 2}");

            // Simplified - dealer hits on 17 or less
            while (dealerHand < 17)
            {
                dealerHand += DealCard();
            }

            Debug.AddOnScreenMessage(-1, 10.0f, Color.LimeGreen, 
                $"👤 Dealer Total: {dealerHand}");

            double winnings = 0;

            if (playerHand > 21)
            {
                Debug.AddOnScreenMessage(-1, 9.0f, Color.Red, "💥 BUST! You went over 21!");
            }
            else if (dealerHand > 21)
            {
                winnings = bet * 2;
                Debug.AddOnScreenMessage(-1, 9.0f, Color.LimeGreen, 
                    "✨ Dealer Bust! You Win! ✨");
            }
            else if (playerHand > dealerHand)
            {
                winnings = bet * 2;
                Debug.AddOnScreenMessage(-1, 9.0f, Color.LimeGreen, 
                    "🏆 You Beat the Dealer! 🏆");
            }
            else if (playerHand == dealerHand)
            {
                winnings = bet;
                Debug.AddOnScreenMessage(-1, 9.0f, Color.DeepSkyBlue, "🤝 Push! Bet Returned!");
            }
            else
            {
                Debug.AddOnScreenMessage(-1, 9.0f, Color.Red, "💔 Dealer Wins!");
            }

            Debug.AddOnScreenMessage(-1, 8.5f, Color.Gold, 
                $"💰 Result: ${winnings:F2}");

            if (winnings > 0)
            {
                player.AddWinnings(winnings);
            }

            player.IncrementGamesPlayed();
        }

        private static int DealCard()
        {
            return (random.Next(13) + 1);
        }
    }

    /// <summary>
    /// Roulette Game - Spin the wheel!
    /// </summary>
    public class RouletteGame
    {
        private static Random random = new Random();

        public static void PlayRoulette(Player player)
        {
            Debug.AddOnScreenMessage(-1, 15.0f, Color.Gold, "╔═══════════════════════════════╗");
            Debug.AddOnScreenMessage(-1, 14.5f, Color.Gold, "║     🎡 ROULETTE WHEEL 🎡      ║");
            Debug.AddOnScreenMessage(-1, 14.0f, Color.Gold, "║    Spin & Win Big! 💎         ║");
            Debug.AddOnScreenMessage(-1, 13.5f, Color.Gold, "╚═══════════════════════════════╝");

            double bet = 200;

            if (!player.PlaceBet(bet))
            {
                Debug.AddOnScreenMessage(-1, 12.0f, Color.Red, "❌ Insufficient balance!");
                return;
            }

            // Spinning animation
            for (int i = 0; i < 5; i++)
            {
                string spinner = i % 2 == 0 ? "🔴" : "⚫";
                Debug.AddOnScreenMessage(-1, 11.0f, Color.Yellow, 
                    $"Spinning: {spinner} {spinner} {spinner}");
            }

            int result = random.Next(37); // 0-36 (European roulette)
            bool isRed = IsRed(result);
            bool isEven = result % 2 == 0 && result > 0;

            Color wheelColor = isRed ? Color.Red : Color.Black;
            string colorName = isRed ? "RED" : "BLACK";

            Debug.AddOnScreenMessage(-1, 10.0f, wheelColor, 
                $"🎡 THE WHEEL STOPS ON: {result} ({colorName})");

            double winnings = 0;

            // Bet on RED
            if (isRed)
            {
                winnings = bet * 2;
                Debug.AddOnScreenMessage(-1, 9.0f, Color.Red, 
                    $"🎉 RED WINS! You Win ${winnings:F2}");
            }
            else
            {
                Debug.AddOnScreenMessage(-1, 9.0f, Color.Black, 
                    $"☠️ BLACK WINS! You Lose ${bet:F2}");
            }

            if (winnings > 0)
            {
                player.AddWinnings(winnings);
            }

            player.IncrementGamesPlayed();
        }

        private static bool IsRed(int number)
        {
            int[] redNumbers = { 1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36 };
            return System.Array.IndexOf(redNumbers, number) >= 0;
        }
    }
}
