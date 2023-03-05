namespace MarketPlace;

public class ProductParams
{
    public string SearchedName { get; }
    public bool InRating { get; }
    public bool InPrice { get; }
    public bool InDes { get; }
    
    public ProductParams(string searchedName, bool inRating, bool inPrice, bool inDes)
    {
        SearchedName = searchedName;
        InRating = inRating;
        InPrice = inPrice;
        InDes = inDes;
    }
}