using System;
using System.Collections;
using System.Collections.Generic;

namespace Spell.Syntax
{
    public enum SyntaxKind
    {
        //Tokens
        BadToken,
        EndOfFileToken,
        NullToken,

        WhitespaceToken,
        NumberToken,
        PlusToken,
        MinusToken,
        StarToken,
        SlashToken,
        OpenParenthesisToken,
        CloseParenthesisToken,
        OpenBraceToken,
        CloseBraceToken,
        EqualsToken,
        EqualsEqualsToken,
        LessToken,
        LessOrEqualsToken,
        GreaterToken,
        GreaterOrEqualsToken,
        BangEqualsToken,
        AmpersandAmpersandToken,
        PipePipeToken,
        BangToken,
        IdentifierToken,

        //Keywords
        FalseKeyword,
        TrueKeyword,
        LetKeyword,
        VarKeyword,
        IfKeyword,
        ElseKeyword,
        WhileKeyword,

        //Statements
        BlockStatement,
        ExpressionStatement,
        VariableDeclaration,
        IfStatement,
        ElseClause,
        WhileStatement,

        //Expressions
        LiteralExpression,
        UnaryExpression,
        BinaryExpression,
        ParenthesizedExpression,
        NameExpression,
        AssignmentExpression,

        //Units
        CompilationUnit,
    }

    public static class SyntaxFacts 
    {
        public static string GetText(SyntaxKind syntaxKind) 
        {
            switch (syntaxKind) 
            {
                case SyntaxKind.PlusToken:                  return "+";
                case SyntaxKind.MinusToken:                 return "-";
                case SyntaxKind.StarToken:                  return "*";
                case SyntaxKind.SlashToken:                 return "/";
                case SyntaxKind.OpenParenthesisToken:       return "(";
                case SyntaxKind.CloseParenthesisToken:      return ")";
                case SyntaxKind.OpenBraceToken:             return "{";
                case SyntaxKind.CloseBraceToken:            return "}";
                case SyntaxKind.EqualsToken:                return "=";
                case SyntaxKind.EqualsEqualsToken:          return "==";
                case SyntaxKind.LessToken:                  return "<";
                case SyntaxKind.LessOrEqualsToken:          return "<=";
                case SyntaxKind.GreaterToken:               return ">";
                case SyntaxKind.GreaterOrEqualsToken:       return ">=";
                case SyntaxKind.BangEqualsToken:            return "!=";
                case SyntaxKind.AmpersandAmpersandToken:    return "&&";
                case SyntaxKind.PipePipeToken:              return "||";
                case SyntaxKind.BangToken:                  return "!";
                case SyntaxKind.FalseKeyword:               return "false";
                case SyntaxKind.TrueKeyword:                return "true";
                case SyntaxKind.LetKeyword:                 return "let";
                case SyntaxKind.VarKeyword:                 return "var";
                case SyntaxKind.IfKeyword:                  return "if";
                case SyntaxKind.ElseKeyword:                return "else";
                case SyntaxKind.WhileKeyword:               return "while";

                default:
                    return null;
            }
        }

        public static IEnumerable<SyntaxKind> GetBinaryOperatorKinds() 
        {
            var kinds = (SyntaxKind[])Enum.GetValues(typeof(SyntaxKind));
            foreach(var kind in kinds) 
            {
                if (GetBinaryOperatorPrecedence(kind) > 0) 
                {
                    yield return kind;
                }
            }
        }

        public static IEnumerable<SyntaxKind> GetUnaryOperatorKinds()
        {
            var kinds = (SyntaxKind[])Enum.GetValues(typeof(SyntaxKind));
            foreach (var kind in kinds)
            {
                if (GetUnaryOperatorPrecedence(kind) > 0)
                {
                    yield return kind;
                }
            }
        }

        public static int GetUnaryOperatorPrecedence(this SyntaxKind syntaxKind) 
        {
            switch (syntaxKind)
            {
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                case SyntaxKind.BangToken:
                    return 6;

                default:
                    return 0;
            }
        }
        public static int GetBinaryOperatorPrecedence(this SyntaxKind syntaxKind)
        {
            switch (syntaxKind)
            {
                case SyntaxKind.StarToken:
                case SyntaxKind.SlashToken:
                    return 5;

                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    return 4;

                case SyntaxKind.LessToken:
                case SyntaxKind.LessOrEqualsToken:
                case SyntaxKind.GreaterToken:
                case SyntaxKind.GreaterOrEqualsToken:
                case SyntaxKind.EqualsEqualsToken:
                case SyntaxKind.BangEqualsToken:
                    return 3;

                case SyntaxKind.AmpersandAmpersandToken:
                    return 2;

                case SyntaxKind.PipePipeToken:
                    return 1;

                default:
                    return 0;
            }
        }

        public static SyntaxKind GetKeywordKind(string text) 
        {
            switch (text) 
            {
                case "false":   return SyntaxKind.FalseKeyword;
                case "true":    return SyntaxKind.TrueKeyword;
                case "let":     return SyntaxKind.LetKeyword;
                case "var":     return SyntaxKind.VarKeyword;
                case "if":      return SyntaxKind.IfKeyword;
                case "else":    return SyntaxKind.ElseKeyword;
                case "while":   return SyntaxKind.WhileKeyword;

                default:        return SyntaxKind.IdentifierToken;
            }
        }
    }
}
