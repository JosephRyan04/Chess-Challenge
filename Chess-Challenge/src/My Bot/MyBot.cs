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
        Move[] moves = board.GetLegalMoves();
        for (int i = 0; i < moves.Length; i++)
        {
            board.MakeMove(moves[i]);
            if (board.IsInCheckmate())
            {
                return moves[i];
            }
            board.UndoMove(moves[i]);
        }


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
            captureMove = moves.MaxBy(x => x.CapturePieceType);
            if (captureMove.CapturePieceType <= 0)
            {
                captureMove = moves.FirstOrDefault(x => x.MovePieceType == PieceType.Queen, moves[0]);
            }
        }
        return captureMove;
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