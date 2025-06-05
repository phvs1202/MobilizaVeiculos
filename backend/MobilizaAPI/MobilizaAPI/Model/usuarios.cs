namespace MobilizaAPI.Model
{
    public class usuarios
    {
        public int id { get; set; }
        public string? nome { get; set; }
        public string? email { get; set; }
        public string? senha { get; set; }
        public int tipo_usuario_id { get; set; }
        public int? curso_id { get; set; }
        public int numero_cnh { get; set; }
        public string categoria_cnh { get; set; }
        public DateTime validade_cnh { get; set; }
        public int status_id { get; set; }
    }
}
