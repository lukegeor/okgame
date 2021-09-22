namespace okgame.Server.Models
{
    public interface IPlayedTile
    {
        Coordinate Coordinate { get; }
        Player Player { get; }

        void MoveCoordinate(Coordinate newCoordinate);
    }
}