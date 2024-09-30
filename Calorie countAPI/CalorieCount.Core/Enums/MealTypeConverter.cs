using System.Text.Json.Serialization;
using System.Text.Json;

namespace CalorieCount.Core.Enums
{
	public class MealTypeConverter : JsonConverter<MealType>
	{
		public override MealType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			return Enum.Parse<MealType>(reader.GetString(), true);
		}

		public override void Write(Utf8JsonWriter writer, MealType value, JsonSerializerOptions options)
		{
			writer.WriteStringValue(value.ToString());
		}
	}

}
