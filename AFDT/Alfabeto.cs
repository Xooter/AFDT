using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFDT
{
	public class Alfabeto
	{
		public List<string> lista_alf = new List<string>();

		//validar si la palabra es longitud 1 y no se repita
		public bool validate_alf(List<string> new_list)
        {
			if (new_list.Count != new_list.Distinct().Count())
				return false;
			foreach (string character in new_list)
			{
				if (character.Length > 1)
					return false;
			}
			return true;
        }

	}
}
