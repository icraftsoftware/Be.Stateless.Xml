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
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using Be.Stateless.Extensions;
using Be.Stateless.Resources;
using Be.Stateless.Xml.Builder;
using FluentAssertions;
using Moq;
using Xunit;

namespace Be.Stateless.Xml
{
	public class XmlBuilderReaderFixture
	{
		public XmlBuilderReaderFixture()
		{
			IdentityTransform = new XslCompiledTransform(true);
			using (var xmlReader = XmlReader.Create(ResourceManager.Load(Assembly.GetExecutingAssembly(), "Be.Stateless.Xml.Xsl.XsltIdentity.xslt")))
			{
				IdentityTransform.Load(xmlReader);
			}
		}

		private XslCompiledTransform IdentityTransform { get; }

		[Fact]
		public void DisposeIXmlElementBuilder()
		{
			var builderMock = new Mock<IDisposable>();
			using (new XmlBuilderReader(builderMock.As<IXmlElementBuilder>().Object)) { }

			builderMock.Verify(b => b.Dispose());
		}

		[Theory]
		[ClassData(typeof(XmlBuilderTestCasesFactory))]
		public void XmlReaderConformanceOnOuterXml(XmlElementBuilder builder, string expected)
		{
			AssertOuterXmlConformance(new XmlBuilderReader(builder), expected.IsNullOrEmpty() ? EmptyXmlReader.Create() : XmlReader.Create(new StringReader(expected)));
			AssertOuterXmlConformance(expected.IsNullOrEmpty() ? EmptyXmlReader.Create() : XmlReader.Create(new StringReader(expected)), new XmlBuilderReader(builder));
			AssertOuterXmlContent(new XmlBuilderReader(builder), expected.IsNullOrEmpty() ? EmptyXmlReader.Create() : XmlReader.Create(new StringReader(expected)));
			AssertOuterXmlInvocations(new XmlBuilderReader(builder), expected.IsNullOrEmpty() ? EmptyXmlReader.Create() : XmlReader.Create(new StringReader(expected)));
		}

		[Theory]
		[ClassData(typeof(XmlBuilderTestCasesFactory))]
		public void XmlReaderConformanceOnRead(XmlElementBuilder builder, string expected)
		{
			AssertReadConformance(new XmlBuilderReader(builder), expected.IsNullOrEmpty() ? EmptyXmlReader.Create() : XmlReader.Create(new StringReader(expected)));
			AssertReadConformance(expected.IsNullOrEmpty() ? EmptyXmlReader.Create() : XmlReader.Create(new StringReader(expected)), new XmlBuilderReader(builder));
		}

		[Theory]
		[ClassData(typeof(XmlBuilderTestCasesFactory))]
		public void XmlReaderConformanceOnTransform(XmlElementBuilder builder, string expected)
		{
			AssertTransformConformance(new XmlBuilderReader(builder), expected.IsNullOrEmpty() ? EmptyXmlReader.Create() : XmlReader.Create(new StringReader(expected)));
			AssertTransformConformance(expected.IsNullOrEmpty() ? EmptyXmlReader.Create() : XmlReader.Create(new StringReader(expected)), new XmlBuilderReader(builder));
			AssertTransformResult(new XmlBuilderReader(builder), expected.IsNullOrEmpty() ? EmptyXmlReader.Create() : XmlReader.Create(new StringReader(expected)));
			AssertTransformInvocations(new XmlBuilderReader(builder), expected.IsNullOrEmpty() ? EmptyXmlReader.Create() : XmlReader.Create(new StringReader(expected)));
		}

		private void AssertOuterXmlConformance(XmlReader actual, XmlReader expected)
		{
			Action act = () => {
				using (var verifier = new XmlReaderConformanceVerifier(actual, expected))
				{
					verifier.MoveToContent();
					verifier.ReadOuterXml();
				}
			};
			act.Should().NotThrow();
		}

		private void AssertOuterXmlContent(XmlReader actual, XmlReader expected)
		{
			using (actual)
			using (expected)
			{
				actual.MoveToContent();
				expected.MoveToContent();
				actual.ReadOuterXml().Should().Be(expected.ReadOuterXml());
			}
		}

		private void AssertOuterXmlInvocations(XmlReader actual, XmlReader expected)
		{
			var actualSpy = new XmlReaderSpy(actual);
			var expectedSpy = new XmlReaderSpy(expected);
			using (actualSpy)
			using (expectedSpy)
			{
				actualSpy.MoveToContent();
				expectedSpy.MoveToContent();
				actualSpy.ReadOuterXml();
				expectedSpy.ReadOuterXml();
			}

			actualSpy.Invocations.Should().Be(expectedSpy.Invocations);
		}

		private void AssertReadConformance(XmlReader actual, XmlReader expected)
		{
			Action act = () => {
				using (var verifier = new XmlReaderConformanceVerifier(expected, actual))
				{
					while (verifier.Read())
					{
						if (verifier.NodeType != XmlNodeType.Element) continue;
						while (verifier.MoveToNextAttribute())
						{
							verifier.ReadAttributeValue();
						}

						verifier.MoveToContent();
					}
				}
			};
			act.Should().NotThrow();
		}

		private void AssertTransformConformance(XmlReader actual, XmlReader expected)
		{
			Action act = () => {
				using (var verifier = new XmlReaderConformanceVerifier(actual, expected))
				{
					using (var writer = XmlWriter.Create(new StringBuilder()))
					{
						IdentityTransform.Transform(verifier, writer);
					}
				}
			};
			act.Should().NotThrow();
		}

		private void AssertTransformResult(XmlReader actual, XmlReader expected)
		{
			using (actual)
			using (expected)
			{
				var expectedBuilder = new StringBuilder();
				using (var writer = XmlWriter.Create(expectedBuilder))
				{
					IdentityTransform.Transform(expected, writer);
				}

				var actualBuilder = new StringBuilder();
				using (var writer = XmlWriter.Create(actualBuilder))
				{
					IdentityTransform.Transform(actual, writer);
				}

				actualBuilder.ToString().Should().Be(expectedBuilder.ToString());
			}
		}

		private void AssertTransformInvocations(XmlReader actual, XmlReader expected)
		{
			var actualSpy = new XmlReaderSpy(actual);
			var expectedSpy = new XmlReaderSpy(expected);
			using (actualSpy)
			using (expectedSpy)
			{
				using (var writer = XmlWriter.Create(new StringBuilder()))
				{
					IdentityTransform.Transform(expectedSpy, writer);
				}

				using (var writer = XmlWriter.Create(new StringBuilder()))
				{
					IdentityTransform.Transform(actualSpy, writer);
				}
			}

			actualSpy.Invocations.Should().Be(expectedSpy.Invocations);
		}
	}
}
