namespace Web.DTOs
{// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Firmness
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class Flavor
    {
        public int? Potency { get; set; }
        public Flavor BerryFlavor { get; set; }
    }

    public class Flavor2
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class Item
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class NaturalGiftType
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class GetPokeByIdResponse
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int? GrowthTime { get; set; }
        public int? MaxHarvest { get; set; }
        public int? NaturalGiftPower { get; set; }
        public int? Size { get; set; }
        public int? Smoothness { get; set; }
        public int? SoilDryness { get; set; }
        //public Firmness Firmness { get; set; }
        public List<Flavor> Flavors { get; set; }
        public Item Item { get; set; }
        public NaturalGiftType NaturalGiftType { get; set; }
    }
}
