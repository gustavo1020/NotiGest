namespace Application.Page
{
    public class Paginated<T> where T : class
    {
        public int CantItemForPage { get; set; }
        public int PageNumber { get; set; }

        public int AllItems { get; set; }
        public IEnumerable<T>? Resultado { get; set; }
    }
}
