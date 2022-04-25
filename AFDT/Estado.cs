using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFDT
{
    public class Estado
    {
        public List<string> lista_estados = new List<string>();
		public List<string> lista_estados_aceptacion = new List<string>();
		public string inicial;


		//validaciones para que no se repitan, sean longitud 1
		public bool validate_estado(List<string> new_list)
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

		//validar si aceptacion pertenece a lista_estados
		public bool validate_estado_aceptacion(List<string> new_list)
		{
			if (new_list.Count != new_list.Distinct().Count() || new_list.Contains(inicial))
				return false;
			foreach (string character in new_list)
			{
				if (character.Length > 1 || !this.lista_estados.Contains(character))
					return false;
			}
			return true;
		}

	}
}
