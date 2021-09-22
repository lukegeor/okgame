namespace okgame.Server.Models
{
    public interface IPlayedTile
    {
        Coordinate Coordinate { get; }
        int PlayerPlayOrder { get; }

        void MoveCoordinate(Coordinate newCoordinate);
    }
}