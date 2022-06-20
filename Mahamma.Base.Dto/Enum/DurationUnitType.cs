namespace Mahamma.Base.Dto.Enum
{
    public class DurationUnitType
    {
        #region Property
        public int Id { get; private set; }
        public string Name { get; private set; }
        #endregion

        #region Enum Values
        public static DurationUnitType Minutes = new(1, nameof(Minutes).ToLowerInvariant());
        public static DurationUnitType Hours = new(2, nameof(Hours).ToLowerInvariant());
        #endregion

        #region CTRS
        public DurationUnitType(int id, string name)
        {
            Id = id;
            Name = name;
        }
        #endregion
    }
}
