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
using System.Xml.XPath;

namespace Be.Stateless.Xml
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1010:Generic interface should also be implemented")]
	public class XPathNodeIteratorDecorator : XPathNodeIterator
	{
		internal XPathNodeIteratorDecorator(XPathNodeIterator decoratedIterator, XPathNavigatorDecorator xPathNavigatorDecorator)
		{
			_decoratedIterator = decoratedIterator ?? throw new ArgumentNullException(nameof(decoratedIterator));
			_xPathNavigatorDecorator = xPathNavigatorDecorator ?? throw new ArgumentNullException(nameof(xPathNavigatorDecorator));
		}

		#region Base Class Member Overrides

		public override XPathNodeIterator Clone()
		{
			return new XPathNodeIteratorDecorator(_decoratedIterator.Clone(), _xPathNavigatorDecorator);
		}

		public override XPathNavigator Current => _xPathNavigatorDecorator.DecorateXPathNavigator(_decoratedIterator.Current);

		public override int CurrentPosition => _decoratedIterator.CurrentPosition;

		public override bool MoveNext()
		{
			return _decoratedIterator.MoveNext();
		}

		#endregion

		private readonly XPathNodeIterator _decoratedIterator;
		private readonly XPathNavigatorDecorator _xPathNavigatorDecorator;
	}
}
