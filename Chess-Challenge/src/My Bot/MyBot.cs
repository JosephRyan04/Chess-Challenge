using System;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using ChessChallenge.API;

public class MyBot : IChessBot
{
    public Move Think(Board board, Timer timer)
    {
        var color = board.IsWhiteToMove; // True is white, False is black
        var currentEval = EvaluateBoard(board, color);
        var nextEval = -1m;
        var maxEval = -1m;   
        Move[] moves = board.GetLegalMoves();
        var moveChoice = new Move();
        for (int i = 0; i < moves.Length; i++)
        {
            board.MakeMove(moves[i]);
            nextEval = EvaluateBoard(board, color);
            if (nextEval >= maxEval)
            {
                maxEval = nextEval;
                moveChoice = moves[i];

            }
            board.UndoMove(moves[i]);
        }

        return maxEval > -1m ? moveChoice : moves[0]; 
    }
    public Move[] MakeMoveAndGetLegalMoves(Move move, Board board)
    {
        board.MakeMove(move);
        var LegalMoves = board.GetLegalMoves();
        board.UndoMove(move);
        return LegalMoves;
    }

    public decimal EvaluateBoard(Board board, bool color)
    {
        if (board.IsInCheckmate())
        {
            return color == board.IsWhiteToMove ? -2147483647 : 2147483647;
        }
        var pieceList = board.GetAllPieceLists();
        var whiteScore = pieceList[0].Count
        + (pieceList[1].Count * 3)
        + (pieceList[2].Count * 3)
        + (pieceList[3].Count * 5)
        + (pieceList[4].Count * 9);

        var blackScore = pieceList[6].Count
        + (pieceList[7].Count * 3)
        + (pieceList[8].Count * 3)
        + (pieceList[9].Count * 5)
        + (pieceList[10].Count * 9);

        return color ? whiteScore / (whiteScore + blackScore) : blackScore / (whiteScore + blackScore);

    }
}