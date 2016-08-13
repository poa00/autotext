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


using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using AutoText.Engine;

namespace AutoText.Helpers.Extensions
{
	public static class Common
	{
		public static string ConcatToString(this List<AutotextInput> input)
		{
			return new string(input.Select(p => p.CharToInput).ToArray());
		}

		public static string EscapeSpecialExpressionChars(this string str)
		{
			StringBuilder sb = new StringBuilder(str);

			for (int i = 0; i < sb.Length; i++)
			{
				if (sb[i] == '{')
				{
					sb = sb.Remove(i, 1);
					sb = sb.Insert(i, "{{}");
					i += 2;
					continue;
				}

				if (sb[i] == '}')
				{
					sb = sb.Remove(i, 1);
					sb = sb.Insert(i, "{}}");
					i += 2;
					continue;
				}

				if (sb[i] == '[')
				{
					sb = sb.Remove(i, 1);
					sb = sb.Insert(i, "{[}");
					i += 2;
					continue;
				}

				if (sb[i] == ']')
				{
					sb = sb.Remove(i, 1);
					sb = sb.Insert(i, "{]}");
					i += 2;
					continue;
				}
			}

			return sb.ToString();
		}
	}
}
