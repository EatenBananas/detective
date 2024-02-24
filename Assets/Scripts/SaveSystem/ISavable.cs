namespace SaveSystem
{
    public interface ISavable
    {
        /// <summary>
        /// The key is used to identify the savable object.
        /// </summary>
        public string Key { get; }
        
        /// <summary>
        /// The default value is the value that will be used if key is not found in the save file.
        /// </summary>
        public object DefaultValue { get; }
        
        /// <summary>
        /// The priority is used to determine the order in which the savable objects are saved.
        /// Smaller values are saved first.
        /// </summary>
        public int Priority { get; }

        /// <summary>
        /// Save the data.
        /// </summary>
        /// <returns>The data that was saved.</returns>
        public object Save();
        
        /// <summary>
        /// Load the data.
        /// </summary>
        /// <param name="value">The data to load.</param>
        public void Load(object value);
    }
}