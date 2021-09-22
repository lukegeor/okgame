namespace okgame.Server.Models
{
    public class PlayedTile : IPlayedTile
    {
        public Coordinate Coordinate { get; private set; }

        public Player Player { get; }

        public PlayedTile(Coordinate coordinate, Player player)
        {
            Coordinate = coordinate;
            Player = player;
        }

        public void MoveCoordinate(Coordinate newCoordinate)
        {
            Coordinate = newCoordinate;
        }
    }
}