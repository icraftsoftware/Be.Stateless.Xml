#region Copyright & License

// Copyright © 2012 - 2020 François Chabot
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using FluentAssertions;
using Xunit;

namespace Be.Stateless.Extensions
{
	public class StringExtensionsFixture
	{
		[Theory]
		[InlineData("name0", true)]
		[InlineData("name-0", true)]
		[InlineData("na-me", true)]
		[InlineData("na.me", true)]
		[InlineData("na.me.0", true)]
		[InlineData("ns0:name", true)]
		[InlineData("ns-0:name-0", true)]
		[InlineData(null, false)]
		[InlineData("", false)]
		[InlineData("-name", false)]
		[InlineData("ns:-name", false)]
		[InlineData(".name", false)]
		[InlineData("ns:.name", false)]
		[InlineData("0name", false)]
		[InlineData("ns:0name", false)]
		[InlineData("0ns:0name", false)]
		[InlineData(":name", false)]
		[InlineData(":name:name", false)]
		[InlineData("ns0::name", false)]
		[InlineData("ns:name:suffix", false)]
		public void IsQName(string qName, bool expectedPredicateResult)
		{
			qName.IsQName().Should().Be(expectedPredicateResult);
		}

		[Fact]
		[SuppressMessage("ReSharper", "StringLiteralTypo")]
		public void ToQName()
		{
			const string content = @"<xsl:stylesheet version='1.0'
	xmlns:xsl='http://www.w3.org/1999/XSL/Transform'
	xmlns:bi='http://schemas.microsoft.com/Sql/2008/05/TableOp/dbo/BatchItems'
	xmlns:tt='http://schemas.microsoft.com/Sql/2008/05/Types/Tables/dbo'
	xmlns:bts='http:=//schemas.microsoft.com/BizTalk/2003/system-properties'
	xmlns:tp='urn:schemas.stateless.be:biztalk:properties:tracking:2012:04'>
</xsl:stylesheet>";

			var navigator = new XPathDocument(new StringReader(content)).CreateNavigator();
			navigator.MoveToFollowing(XPathNodeType.Element);

			"no-namespace".ToQName(navigator).Should().Be(new XmlQualifiedName("no-namespace", null));
			"bts:OutboundTransportLocation".ToQName(navigator).Should()
				.Be(new XmlQualifiedName("OutboundTransportLocation", "http:=//schemas.microsoft.com/BizTalk/2003/system-properties"));
			"tp:MessagingStepActivityID".ToQName(navigator).Should()
				.Be(new XmlQualifiedName("MessagingStepActivityID", "urn:schemas.stateless.be:biztalk:properties:tracking:2012:04"));
			Action act = () => ":MessagingStepActivityID".ToQName(navigator);
			act.Should().Throw<ArgumentException>()
				.WithMessage("':MessagingStepActivityID' is not a valid XML qualified name.*")
				.Where(e => e.ParamName == "qName");
		}

		[Theory]
		[InlineData("0value", false, null, null)]
		[InlineData("value", true, "", "value")]
		[InlineData("ns:value", true, "ns", "value")]
		public void TryParseQName(string qName, bool expectedParsingSuccessResult, string expectedPrefix, string expectedLocalPart)
		{
			qName.TryParseQName(out var actualPrefix, out var actualLocalPart).Should().Be(expectedParsingSuccessResult);
			actualPrefix.Should().Be(expectedPrefix);
			actualLocalPart.Should().Be(expectedLocalPart);
		}
	}
}
