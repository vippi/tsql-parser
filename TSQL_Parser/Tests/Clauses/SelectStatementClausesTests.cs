﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using TSQL;
using TSQL.Clauses;
using TSQL.Clauses.Parsers;
using TSQL.Tokens;

namespace Tests.Clauses
{
	[TestFixture(Category = "Clause Parsing")]
	public class SelectStatementClausesTests
	{
		[Test]
		public void SelectClause_StopAtFrom()
		{
			using (StringReader reader = new StringReader(@"select a from b;"))
			using (IEnumerator<TSQLToken> tokenizer = new TSQLTokenizer(reader))
			{
				Assert.IsTrue(tokenizer.MoveNext());
				TSQLSelectClause select = new TSQLSelectClauseParser().Parse(tokenizer);
				Assert.AreEqual(2, select.Tokens.Count);
				Assert.AreEqual(TSQLKeywords.FROM, tokenizer.Current.AsKeyword.Keyword);
			}
		}
	}
}
