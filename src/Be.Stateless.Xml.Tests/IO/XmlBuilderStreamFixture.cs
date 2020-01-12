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
using System.IO;
using Be.Stateless.Xml.Builder;
using FluentAssertions;
using Moq;
using Xunit;

namespace Be.Stateless.IO
{
	public class XmlBuilderStreamFixture
	{
		[Fact]
		public void DisposeIXmlElementBuilder()
		{
			var builderMock = new Mock<IDisposable>();
			using (new StreamReader(new XmlBuilderStream(builderMock.As<IXmlElementBuilder>().Object))) { }

			builderMock.Verify(b => b.Dispose());
		}

		[Theory]
		[ClassData(typeof(XmlBuilderTestCasesFactory))]
		public void ValidateXmlStreamContent(XmlElementBuilder builder, string expected)
		{
			using (var reader = new StreamReader(new XmlBuilderStream(builder)))
			{
				reader.ReadToEnd().Should().Be(expected);
			}
		}
	}
}
