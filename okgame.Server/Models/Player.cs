namespace okgame.Server.Models
{
    public class Player
    {
        public const int StartingTiles = 16;

        public int PlayOrder { get; set; }

        public int TilesLeft { get; set; }

        public User User { get; set; }

        public Player(User user, int playOrder)
        {
            TilesLeft = StartingTiles;
            PlayOrder = playOrder;
            User = user;
        }

        public override string ToString()
        {
            return "Player " + PlayOrder;
        }
    }
}