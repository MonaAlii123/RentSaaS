namespace employee.DTOs
{
    public class PaginationDTO<T>
    {
        public int entitiesNumber { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public List<T> entites { get; set; }
    }
}
