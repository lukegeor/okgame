namespace okgame.Server.Models
{
    public class PlayedTile : IPlayedTile
    {
        public Coordinate Coordinate { get; private set; }

        public int PlayerPlayOrder { get; }

        public PlayedTile(Coordinate coordinate, int playerPlayOrder)
        {
            Coordinate = coordinate;
            PlayerPlayOrder = playerPlayOrder;
        }

        public void MoveCoordinate(Coordinate newCoordinate)
        {
            Coordinate = newCoordinate;
        }

        public override string ToString() => $"{PlayerPlayOrder} {Coordinate}";
    }
}