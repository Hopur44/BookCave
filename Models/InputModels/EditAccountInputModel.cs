namespace BookCave.Models.InputModels
{
    public class EditAccountInputModel
    {
        public int Id {get; set;}

        public string Name {get;set;}
        
        public string Address  {get; set;}

        public string Image {get; set;}

        public string FavouriteBook {get; set;}
    }
}