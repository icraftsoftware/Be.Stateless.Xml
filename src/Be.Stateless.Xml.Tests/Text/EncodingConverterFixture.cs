#region Copyright & License

// Copyright © 2012 - 2021 François Chabot
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
using System.Text;
using FluentAssertions;
using Xunit;
using static FluentAssertions.FluentActions;

namespace Be.Stateless.Text
{
	public class EncodingConverterFixture
	{
		[Fact]
		public void CanConvertFrom()
		{
			var sut = new EncodingConverter();
			sut.CanConvertFrom(typeof(string)).Should().BeTrue();
		}

		[Fact]
		public void CanConvertTo()
		{
			var sut = new EncodingConverter();
			sut.CanConvertTo(typeof(string)).Should().BeTrue();
		}

		[Fact]
		public void ConvertFromThrowsWhenEncodingIsUnknown()
		{
			var sut = new EncodingConverter();
			Invoking(() => sut.ConvertFrom("utf-48"))
				.Should().Throw<ArgumentException>().WithMessage("'utf-48' is not a supported encoding name.*");
		}

		[Fact]
		public void ConvertFromThrowsWhenInvalidFormat()
		{
			const string value = "utf-8, bam";
			var sut = new EncodingConverter();
			Invoking(() => sut.ConvertFrom(value))
				.Should().Throw<NotSupportedException>().WithMessage($"'{value}' format is invalid and cannot be parsed into a {nameof(Encoding)}.");
		}

		[Fact]
		public void ConvertFromThrowsWhenNullOrEmptyString()
		{
			var sut = new EncodingConverter();
			Invoking(() => sut.ConvertFrom(""))
				.Should().Throw<NotSupportedException>().WithMessage($"Cannot parse a null or empty string into a {nameof(Encoding)}.");
		}

		[Fact]
		[SuppressMessage("ReSharper", "PossibleNullReferenceException")]
		public void ConvertFromWithBom()
		{
			var encoding = new UTF8Encoding(true);
			var displayName = $"{encoding.WebName} {EncodingConverter.ENCODING_SIGNATURE_MODIFIER}";

			var sut = new EncodingConverter();
			var convertedObject = sut.ConvertFrom(displayName);

			convertedObject.Should().BeEquivalentTo(encoding);
			convertedObject.Should().BeOfType<UTF8Encoding>();
			var convertedEncoding = convertedObject as UTF8Encoding;
			convertedEncoding.WebName.Should().Be(encoding.WebName);
			convertedEncoding.GetPreamble().Length.Should().BeGreaterThan(0);
		}

		[Fact]
		[SuppressMessage("ReSharper", "PossibleNullReferenceException")]
		public void ConvertFromWithBomIsCaseInsensitive()
		{
			var encoding = new UTF8Encoding(true);
			var displayName = $"{encoding.WebName} {EncodingConverter.ENCODING_SIGNATURE_MODIFIER.ToUpper()}";

			var sut = new EncodingConverter();
			var convertedObject = sut.ConvertFrom(displayName);

			convertedObject.Should().BeEquivalentTo(encoding);
			convertedObject.Should().BeOfType<UTF8Encoding>();
			var convertedEncoding = convertedObject as UTF8Encoding;
			convertedEncoding.WebName.Should().Be(encoding.WebName);
			convertedEncoding.GetPreamble().Length.Should().BeGreaterThan(0);
		}

		[Fact]
		[SuppressMessage("ReSharper", "PossibleNullReferenceException")]
		public void ConvertFromWithoutBom()
		{
			var encoding = new UTF8Encoding();
			var displayName = $"{encoding.WebName}";

			var sut = new EncodingConverter();
			var convertedObject = sut.ConvertFrom(displayName);

			convertedObject.Should().BeEquivalentTo(encoding);
			convertedObject.Should().BeOfType<UTF8Encoding>();
			var convertedEncoding = convertedObject as UTF8Encoding;
			convertedEncoding.WebName.Should().Be(encoding.WebName);
			convertedEncoding.GetPreamble().Length.Should().Be(0);
		}

		[Fact]
		public void ConvertToWithBom()
		{
			var encoding = new UTF8Encoding(true);

			var sut = new EncodingConverter();
			var displayName = sut.ConvertTo(encoding, typeof(string));

			displayName.Should().Be($"{encoding.WebName} {EncodingConverter.ENCODING_SIGNATURE_MODIFIER}");
		}

		[Fact]
		public void ConvertToWithoutBom()
		{
			var encoding = new UTF8Encoding();

			var sut = new EncodingConverter();
			var displayName = sut.ConvertTo(encoding, typeof(string));

			displayName.Should().Be($"{encoding.WebName}");
		}
	}
}
