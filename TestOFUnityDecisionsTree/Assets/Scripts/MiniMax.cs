using System.Collections.Generic;

public class MiniMax
{
    // minimax for the O player, returns an array with [value, move] information
    public static int[] Minimax(string[] state, int depth, bool maximizingPlayer)
    {
        //IMPLEMENT minimax

        if (depth == 0 || IsTerminal(state))
        {
            var valueState = HeuristicValueOfState(state, "O");

            return new[] { valueState, 0 };
        }

        if (maximizingPlayer)
        {
            var bestValue = int.MinValue;
            var bestAction = -1;

            var moves = PossibleMoves(state);

            foreach (var move in moves)
            {
                var temp = CalculateNewState(state, "O", move);
                var miniMax = Minimax(temp, depth - 1, false);
                if (bestValue < miniMax[0])
                {
                    bestValue = miniMax[0];
                    bestAction = move;
                }
            }

            return new[] { bestValue, bestAction };
        }
        else
        {
            var bestValue = int.MaxValue;
            var bestAction = -1;
            var moves = PossibleMoves(state);

            foreach (var move in moves)
            {
                var temp = CalculateNewState(state, "X", move);
                var miniMax = Minimax(temp, depth - 1, true);
                if (bestValue > miniMax[0])
                {
                    bestValue = miniMax[0];
                    bestAction = move;
                }
            }

            return new[] { bestValue, bestAction };
        }

        return new[] { -1, -1 };
    }

    // calculate the heuristic value of the state
    private static int HeuristicValueOfState(string[] state)
    {
        if (IsTerminal(state, "X")) return -1;
        if (IsTerminal(state, "O")) return 1;
        return 0;


        //IMPLEMENT A HEURISTIC 
    }

    // calculate the heuristic value of the state
    private static int HeuristicValueOfState(string[] state, string player)
    {
        // Define opponent
        var opponent = player == "O" ? "X" : "O";

        // Weights for heuristics
        var centerWeight = 3;
        var cornerWeight = 2;
        var edgeWeight = 1;
        var twoInARowWeight = 10;

        // Calculate center control
        var value = 0;
        if (state[4] == player) value += centerWeight;
        if (state[4] == opponent) value -= centerWeight;

        // Calculate corner control
        int[] corners = { 0, 2, 6, 8 };
        foreach (var corner in corners)
        {
            if (state[corner] == player) value += cornerWeight;
            if (state[corner] == opponent) value -= cornerWeight;
        }

        // Calculate edge control
        int[] edges = { 1, 3, 5, 7 };
        foreach (var edge in edges)
        {
            if (state[edge] == player) value += edgeWeight;
            if (state[edge] == opponent) value -= edgeWeight;
        }

        // Evaluate rows, columns, and diagonals
        var lines = new[]
        {
            new[] { 0, 1, 2 }, // Top row
            new[] { 3, 4, 5 }, // Middle row
            new[] { 6, 7, 8 }, // Bottom row
            new[] { 0, 3, 6 }, // Left column
            new[] { 1, 4, 7 }, // Middle column
            new[] { 2, 5, 8 }, // Right column
            new[] { 0, 4, 8 }, // Diagonal 1
            new[] { 2, 4, 6 } // Diagonal 2
        };

        foreach (var line in lines)
        {
            var playerCount = 0;
            var opponentCount = 0;
            var emptyCount = 0;

            foreach (var index in line)
                if (state[index] == player) playerCount++;
                else if (state[index] == opponent) opponentCount++;
                else emptyCount++;

            // Offensive scoring: prioritize winning opportunities
            if (playerCount == 2 && emptyCount == 1) value += twoInARowWeight;
            if (opponentCount == 2 && emptyCount == 1)
                // Defensive scoring: block opponent's winning chances
                value -= twoInARowWeight;
        }

        // Return the final heuristic value
        return value;
    }

    //Calculate new state starting from the current one, the player making the move, and where it wants to place its token
    public static string[] CalculateNewState(string[] state, string player, int tile)
    {
        var clone = (string[])state.Clone();
        //IMPLEMENT: calculate new state
        clone[tile] = player;

        return clone;
    }

    // returns a list of all possible moves that can be currently made
    private static List<int> PossibleMoves(string[] state)
    {
        var moves = new List<int>();
        //IMPLEMENT: calculate all possible moves that can be made from the specified state (the squares in which you can put something in)
        for (var i = 0; i < state.Length; i++)
            if (state[i] == ".")
                moves.Add(i);

        return moves;
    }

    //returns true if a player has won or if there is a draw, so the state cannot be expanded anymore
    private static bool IsTerminal(string[] state)
    {
        if (IsTerminal(state, "X"))
            return true;
        if (IsTerminal(state, "O"))
            return true;

        var foundEmptySpot = false;
        foreach (var s in state)
            if (s == ".")
            {
                foundEmptySpot = true;
                break;
            }

        if (!foundEmptySpot)
            return true;
        return false;
    }

    //returns true if specified player has won
    private static bool IsTerminal(string[] state, string player)
    {
        if (state[0] == player && state[1] == player && state[2] == player) return true;

        if (state[3] == player && state[4] == player && state[5] == player) return true;

        if (state[6] == player && state[7] == player && state[8] == player) return true;

        if (state[0] == player && state[3] == player && state[6] == player) return true;

        if (state[1] == player && state[4] == player && state[7] == player) return true;

        if (state[2] == player && state[5] == player && state[8] == player) return true;

        if (state[0] == player && state[4] == player && state[8] == player) return true;

        if (state[2] == player && state[4] == player && state[6] == player) return true;
        return false;
    }
}