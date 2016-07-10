﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TSQL.Tokens;

namespace TSQL.Clauses.Parsers
{
	public class TSQLHavingClauseParser : ITSQLClauseParser
	{
		public TSQLHavingClause Parse(TSQLTokenizer tokenizer)
		{
			TSQLHavingClause having = new TSQLHavingClause();

            if (
                tokenizer.Current == null ||
                tokenizer.Current.Type != TSQLTokenType.Keyword ||
                tokenizer.Current.AsKeyword.Keyword != TSQLKeywords.HAVING)
            {
                throw new ApplicationException("HAVING expected.");
            }

            having.Tokens.Add(tokenizer.Current);

            // subqueries
            int nestedLevel = 0;

			while (
				tokenizer.Read() &&
				!(
					tokenizer.Current.Type == TSQLTokenType.Character &&
					tokenizer.Current.AsCharacter.Character == TSQLCharacters.Semicolon
				) &&
				(
					nestedLevel > 0 ||
					!(
						tokenizer.Current.Type == TSQLTokenType.Keyword &&
						(
							tokenizer.Current.AsKeyword.Keyword == TSQLKeywords.UNION ||
							tokenizer.Current.AsKeyword.Keyword == TSQLKeywords.EXCEPT ||
							tokenizer.Current.AsKeyword.Keyword == TSQLKeywords.INTERSECT ||
							tokenizer.Current.AsKeyword.Keyword == TSQLKeywords.ORDER ||
							tokenizer.Current.AsKeyword.Keyword == TSQLKeywords.FOR ||
							tokenizer.Current.AsKeyword.Keyword == TSQLKeywords.OPTION
						)
					)
				))
			{
				having.Tokens.Add(tokenizer.Current);

				if (tokenizer.Current.Type == TSQLTokenType.Character)
				{
					TSQLCharacters character = tokenizer.Current.AsCharacter.Character;

					if (character == TSQLCharacters.OpenParentheses)
					{
						// should we recurse for subqueries?
						nestedLevel++;
					}
					else if (character == TSQLCharacters.CloseParentheses)
					{
						nestedLevel--;
					}
				}
			}

			return having;
		}

		TSQLClause ITSQLClauseParser.Parse(TSQLTokenizer tokenizer)
		{
			return Parse(tokenizer);
		}
	}
}