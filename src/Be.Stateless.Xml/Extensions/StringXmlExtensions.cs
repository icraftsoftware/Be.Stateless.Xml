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
using System.Text.RegularExpressions;
using System.Xml;

namespace Be.Stateless.Extensions
{
	[SuppressMessage("ReSharper", "UnusedType.Global", Justification = "Public API.")]
	public static class StringXmlExtensions
	{
		/// <summary>
		/// Verifies that the <paramref name="qName"/> string is a valid qualified name according to the W3C Extended Markup
		/// Language recommendation.
		/// </summary>
		/// <param name="qName">
		/// The QName to verify.
		/// </param>
		/// <returns>
		/// <c>true</c> if it is a valid qualified name; <c>false</c> otherwise.
		/// </returns>
		/// <seealso href="http://www.w3.org/TR/2009/REC-xml-names-20091208/#ns-qualnames"/>
		[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Public API.")]
		public static bool IsQName(this string qName)
		{
			// could have used http://msdn.microsoft.com/en-us/library/system.xml.xmlconvert.verifyncname.aspx but it is not a
			// predicate and rather throws an exception instead; see also XmlReader.IsName and XmlReader.IsNameToken
			return TryParseQName(qName, out _, out _);
		}

		/// <summary>
		/// Try to parse a qualified name and extracts its prefix and local parts.
		/// </summary>
		/// <param name="qName">
		/// The qualified to parse.
		/// </param>
		/// <param name="prefix">
		/// The prefix part of the qName if it is a valid qualified name; <see cref="string.Empty"/> if there is no prefix part.
		/// </param>
		/// <param name="localPart">
		/// The local part of the qName if it is a valid qualified name.
		/// </param>
		/// <returns>
		/// <c>true</c> if it is a valid qualified name; <c>false</c> otherwise.
		/// </returns>
		/// <seealso href="http://www.w3.org/TR/xml-names/#ns-qualnames">Qualified Names</seealso>
		[SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Public API.")]
		[SuppressMessage("ReSharper", "CommentTypo")]
		public static bool TryParseQName(this string qName, out string prefix, out string localPart)
		{
			// http://www.w3.org/TR/xml-names/#NT-NCName
			// see also Character Class Subtraction (https://docs.microsoft.com/en-us/dotnet/standard/base-types/character-classes-in-regular-expressions)
			const string ncNamePattern = @"[\w-[\d]][\w\-\.]*";
			// Qualified Names, see http://www.w3.org/TR/xml-names/#ns-qualnames
			const string qNamePattern = @"^(?:(?<prefix>" + ncNamePattern + @")\:)?(?<localPart>" + ncNamePattern + ")$";
			var match = Regex.Match(qName ?? string.Empty, qNamePattern);
			if (match.Success)
			{
				prefix = match.Groups["prefix"].Value;
				localPart = match.Groups["localPart"].Value;
				return true;
			}
			prefix = localPart = null;
			return false;
		}

		/// <summary>
		/// Given a qualified name <paramref name="qName"/>, e.g. <c>ns:name</c> where <c>ns</c> is an xmlns prefix and
		/// <c>name</c> is an node name, and a <see cref="IXmlNamespaceResolver"/> <paramref name="namespaceResolver"/>, parses
		/// the <paramref name="qName"/> string to return its typed <see cref="XmlQualifiedName"/> equivalent.
		/// </summary>
		/// <param name="qName">
		/// The qName string to parse.
		/// </param>
		/// <param name="namespaceResolver">
		/// The <see cref="IXmlNamespaceResolver"/> to use to resolve the xmlns prefix of the <paramref name="qName"/>.
		/// </param>
		/// <returns>
		/// The typed <see cref="XmlQualifiedName"/> equivalent of the <paramref name="qName"/>.
		/// </returns>
		[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Public API.")]
		public static XmlQualifiedName ToQName(this string qName, IXmlNamespaceResolver namespaceResolver)
		{
			if (qName.IsNullOrEmpty()) throw new ArgumentNullException(nameof(qName));
			if (namespaceResolver == null) throw new ArgumentNullException(nameof(namespaceResolver));
			var success = TryParseQName(qName, out var prefix, out var localPart);
			if (!success) throw new ArgumentException($"'{qName}' is not a valid XML qualified name.", nameof(qName));
			var ns = namespaceResolver.LookupNamespace(prefix);
			return new XmlQualifiedName(localPart, ns);
		}
	}
}
