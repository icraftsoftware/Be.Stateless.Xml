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

using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Xml;
using Be.Stateless.Xml.Serialization;
using Be.Stateless.Xml.Serialization.Extensions;

namespace Be.Stateless.Extensions
{
	[SuppressMessage("ReSharper", "UnusedType.Global", Justification = "Public API.")]
	public static class ObjectXmlExtensions
	{
		/// <summary>
		/// Serialize an <paramref name="object"/> instance to a one-line compact XML string.
		/// </summary>
		/// <typeparam name="T">
		/// The type of the <paramref name="object"/> to serialize.
		/// </typeparam>
		/// <param name="object">
		/// The object instance to serialize.
		/// </param>
		/// <returns>
		/// The XML serialization string of <paramref name="object"/>.
		/// </returns>
		[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Public API.")]
		public static string ToXml<T>(this T @object)
		{
			using (var stringWriter = new StringWriter())
			using (var writer = XmlWriter.Create(stringWriter, new() { Indent = false, Encoding = Encoding.UTF8, OmitXmlDeclaration = true }))
			{
				var serializer = CachingXmlSerializerFactory.Create(typeof(T));
				serializer.SerializeWithoutDefaultNamespaces(writer, @object);
				return stringWriter.ToString();
			}
		}
	}
}
