﻿namespace Api.Models
{
    public partial class Cursos
    {
        public int CursoId { get; set; }
        public string Nome { get; set; }
        public int Duracao { get; set; }
        public string DataLimiteMatricula { get; set; }
        public string Custo { get; set; }
        public string DisciplinasAssociadas { get; set; }
    }
}
