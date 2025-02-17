﻿/*This file is part of AutoText.

Copyright © 2016 Alexander Litvinov

AutoText is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 */

using System.Collections.Generic;
using System.Xml.Serialization;

namespace AutoText.Model.Configuration
{
	public class AutotextRuleConfiguration
	{
		[XmlElement("abbreviation")]
		public AutotextRuleAbbreviation Abbreviation { get;  set; }
		[XmlElement("removeAbbr")]
		public bool RemoveAbbr { get; set; }
		[XmlElement("phrase")]
		public string Phrase { get;  set; }
		[XmlElement("macros")]
		public AutotextRuleMacrosMode Macros { get; set; }
		[XmlElement("description")]
		public string Description { get;  set; }
		[XmlElement("specificPrograms")]
		public AutotextRuleSpecificPrograms SpecificPrograms { get; set; }

		[XmlArray("triggers")]
		[XmlArrayItem("item", typeof(AutotextRuleTrigger))]
		public List<AutotextRuleTrigger> Triggers { get;  set; }
	}
}