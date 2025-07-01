using System;
using System.Linq;
using System.Text.RegularExpressions;
using ChessChallenge.API;

public class MyBot : IChessBot
{
    public Move Think(Board board, Timer timer)
    {
        Move[] moves = board.GetLegalMoves();
        var captureMove = new Move();
        for (int i = 0; i < moves.Length; i++)
        {
            board.MakeMove(moves[i]);
            if (board.IsInCheck())
            {
                board.UndoMove(moves[i]);
                return moves[i];
            }
            else board.UndoMove(moves[i]);
        }
        captureMove = moves.FirstOrDefault(x => x.CapturePieceType == PieceType.King, captureMove);
        if (captureMove == new Move())
        {
            captureMove = moves.FirstOrDefault(x => x.IsCapture, new Move());
            if (captureMove == new Move())
            {
                captureMove = moves.FirstOrDefault(x => x.MovePieceType == PieceType.Queen, moves[0]);
            }
        }
        return captureMove;
    }
}