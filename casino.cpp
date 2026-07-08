#include <iostream>
#include <string>
#include <vector>
#include <cstdlib>
#include <ctime>
#include <iomanip>
#include <algorithm>

using namespace std;

// Player class
class Player {
private:
    string name;
    double balance;
    double totalWinnings;
    int gamesPlayed;

public:
    Player(string playerName, double initialBalance = 1000.0)
        : name(playerName), balance(initialBalance), totalWinnings(0), gamesPlayed(0) {}

    string getName() const { return name; }
    double getBalance() const { return balance; }
    double getTotalWinnings() const { return totalWinnings; }
    int getGamesPlayed() const { return gamesPlayed; }

    void addBalance(double amount) {
        balance += amount;
        if (amount > 0) totalWinnings += amount;
    }

    bool placeBet(double amount) {
        if (amount <= balance && amount > 0) {
            balance -= amount;
            return true;
        }
        return false;
    }

    void incrementGames() { gamesPlayed++; }

    void displayStats() const {
        cout << "\n========== PLAYER STATS ==========" << endl;
        cout << "Name: " << name << endl;
        cout << "Current Balance: $" << fixed << setprecision(2) << balance << endl;
        cout << "Total Winnings: $" << totalWinnings << endl;
        cout << "Games Played: " << gamesPlayed << endl;
        cout << "=================================\n" << endl;
    }
};

// Casino Games Base Class
class CasinoGame {
protected:
    Player* player;

public:
    CasinoGame(Player* p) : player(p) {}
    virtual ~CasinoGame() {}
    virtual void play() = 0;
};

// Coin Flip Game
class CoinFlip : public CasinoGame {
public:
    CoinFlip(Player* p) : CasinoGame(p) {}

    void play() override {
        cout << "\n========== COIN FLIP GAME ==========" << endl;
        cout << "Guess heads (H) or tails (T)!" << endl;

        double bet;
        cout << "Enter your bet ($): ";
        cin >> bet;

        if (!player->placeBet(bet)) {
            cout << "Insufficient balance!" << endl;
            return;
        }

        string guess;
        cout << "Enter your guess (H/T): ";
        cin >> guess;
        transform(guess.begin(), guess.end(), guess.begin(), ::toupper);

        int flip = rand() % 2;
        string result = (flip == 0) ? "H" : "T";

        cout << "The coin landed on: " << result << endl;

        if (guess == result) {
            double winnings = bet * 2;
            player->addBalance(winnings);
            cout << "You win! $" << winnings << endl;
        } else {
            cout << "You lose! Better luck next time." << endl;
        }

        player->incrementGames();
    }
};

// Dice Roll Game
class DiceRoll : public CasinoGame {
public:
    DiceRoll(Player* p) : CasinoGame(p) {}

    void play() override {
        cout << "\n========== DICE ROLL GAME ==========" << endl;
        cout << "Roll two dice. If you roll 7 or 11, you win!" << endl;

        double bet;
        cout << "Enter your bet ($): ";
        cin >> bet;

        if (!player->placeBet(bet)) {
            cout << "Insufficient balance!" << endl;
            return;
        }

        int dice1 = (rand() % 6) + 1;
        int dice2 = (rand() % 6) + 1;
        int total = dice1 + dice2;

        cout << "Dice 1: " << dice1 << endl;
        cout << "Dice 2: " << dice2 << endl;
        cout << "Total: " << total << endl;

        if (total == 7 || total == 11) {
            double winnings = bet * 3;
            player->addBalance(winnings);
            cout << "You win! $" << winnings << endl;
        } else {
            cout << "You lose! Better luck next time." << endl;
        }

        player->incrementGames();
    }
};

// Blackjack Game (Simplified)
class Blackjack : public CasinoGame {
private:
    int dealerHand;
    int playerHand;

    int dealCard() {
        return (rand() % 13) + 1;  // 1-13 representing cards
    }

    int cardValue(int card) {
        if (card > 10) return 10;
        if (card == 1) return 11;
        return card;
    }

public:
    Blackjack(Player* p) : CasinoGame(p), dealerHand(0), playerHand(0) {}

    void play() override {
        cout << "\n========== BLACKJACK GAME ==========" << endl;
        cout << "Get as close to 21 as possible without going over!" << endl;

        double bet;
        cout << "Enter your bet ($): ";
        cin >> bet;

        if (!player->placeBet(bet)) {
            cout << "Insufficient balance!" << endl;
            return;
        }

        // Initial deal
        playerHand = cardValue(dealCard()) + cardValue(dealCard());
        dealerHand = cardValue(dealCard());

        cout << "Your hand: " << playerHand << endl;
        cout << "Dealer showing: " << dealerHand << endl;

        // Player's turn
        string hit;
        while (playerHand < 21) {
            cout << "Hit (H) or Stand (S)? ";
            cin >> hit;
            transform(hit.begin(), hit.end(), hit.begin(), ::toupper);

            if (hit == "H") {
                int newCard = cardValue(dealCard());
                playerHand += newCard;
                cout << "You drew: " << newCard << " | Your total: " << playerHand << endl;
            } else {
                break;
            }
        }

        // Dealer's turn
        dealerHand += cardValue(dealCard());
        cout << "Dealer's hand: " << dealerHand << endl;

        // Determine winner
        if (playerHand > 21) {
            cout << "You bust! You lose." << endl;
        } else if (dealerHand > 21) {
            double winnings = bet * 2;
            player->addBalance(winnings);
            cout << "Dealer busts! You win! $" << winnings << endl;
        } else if (playerHand > dealerHand) {
            double winnings = bet * 2;
            player->addBalance(winnings);
            cout << "You win! $" << winnings << endl;
        } else if (playerHand < dealerHand) {
            cout << "Dealer wins! You lose." << endl;
        } else {
            player->addBalance(bet);
            cout << "Push! Your bet is returned." << endl;
        }

        player->incrementGames();
    }
};

// Slot Machine Game
class SlotMachine : public CasinoGame {
private:
    string spin() {
        vector<string> symbols = {"CHERRY", "BELL", "LEMON", "ORANGE", "BAR", "SEVEN"};
        return symbols[rand() % symbols.size()];
    }

public:
    SlotMachine(Player* p) : CasinoGame(p) {}

    void play() override {
        cout << "\n========== SLOT MACHINE ==========" << endl;
        cout << "Match three symbols to win!" << endl;

        double bet;
        cout << "Enter your bet ($): ";
        cin >> bet;

        if (!player->placeBet(bet)) {
            cout << "Insufficient balance!" << endl;
            return;
        }

        string reel1 = spin();
        string reel2 = spin();
        string reel3 = spin();

        cout << "\n[" << reel1 << "] [" << reel2 << "] [" << reel3 << "]\n" << endl;

        if (reel1 == reel2 && reel2 == reel3) {
            double multiplier = (reel1 == "SEVEN") ? 10 : 5;
            double winnings = bet * multiplier;
            player->addBalance(winnings);
            cout << "JACKPOT! You win! $" << winnings << endl;
        } else if (reel1 == reel2 || reel2 == reel3) {
            double winnings = bet * 2;
            player->addBalance(winnings);
            cout << "Two matches! You win! $" << winnings << endl;
        } else {
            cout << "No matches. You lose!" << endl;
        }

        player->incrementGames();
    }
};

// Main Casino Application
class Casino {
private:
    Player* player;

    void displayMainMenu() {
        cout << "\n========== WELCOME TO CASINO ==========" << endl;
        cout << "1. Coin Flip" << endl;
        cout << "2. Dice Roll" << endl;
        cout << "3. Blackjack" << endl;
        cout << "4. Slot Machine" << endl;
        cout << "5. View Player Stats" << endl;
        cout << "6. Exit Casino" << endl;
        cout << "======================================\n" << endl;
    }

public:
    Casino() : player(nullptr) {}

    ~Casino() {
        if (player) delete player;
    }

    void startCasino() {
        srand(static_cast<unsigned>(time(0)));

        cout << "\n========== CASINO APPLICATION ==========" << endl;
        string playerName;
        cout << "Welcome! Enter your name: ";
        getline(cin, playerName);

        double initialBalance;
        cout << "Enter your starting balance ($): ";
        cin >> initialBalance;
        cin.ignore();

        player = new Player(playerName, initialBalance);

        cout << "\nWelcome, " << player->getName() << "!" << endl;
        cout << "Starting balance: $" << fixed << setprecision(2) << player->getBalance() << endl;

        int choice;
        bool running = true;

        while (running && player->getBalance() > 0) {
            displayMainMenu();
            cout << "Current Balance: $" << fixed << setprecision(2) << player->getBalance() << endl;
            cout << "Select a game (1-6): ";
            cin >> choice;
            cin.ignore();

            CasinoGame* game = nullptr;

            switch (choice) {
                case 1:
                    game = new CoinFlip(player);
                    game->play();
                    delete game;
                    break;
                case 2:
                    game = new DiceRoll(player);
                    game->play();
                    delete game;
                    break;
                case 3:
                    game = new Blackjack(player);
                    game->play();
                    delete game;
                    break;
                case 4:
                    game = new SlotMachine(player);
                    game->play();
                    delete game;
                    break;
                case 5:
                    player->displayStats();
                    break;
                case 6:
                    running = false;
                    break;
                default:
                    cout << "Invalid choice. Please try again." << endl;
            }
        }

        if (player->getBalance() <= 0) {
            cout << "\nYou've run out of money! Game Over." << endl;
        }

        cout << "\n========== FINAL STATS ==========" << endl;
        player->displayStats();
        cout << "Thank you for playing at our casino!" << endl;
    }
};

// Main function
int main() {
    Casino casino;
    casino.startCasino();
    return 0;
}
