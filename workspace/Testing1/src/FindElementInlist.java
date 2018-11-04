
import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

public class FindElementInlist {

	public static void main(String[] args) {
		// Remove nulls from List
		List<String> stringList = new ArrayList<String>();
		stringList.add("Soccer");
		stringList.add("Rugby");
		stringList.add(null);
		stringList.add("Badminton");
		stringList.add(null);
		stringList.add("Golf");
		stringList.add(null);
		stringList.add("Tennis");
		System.out.println(stringList);
		
		//Example 1 
		//Finding element in list
		final String query = "badminton";
		System.out.println("Finding " + query + " in games");

		Optional<String> queryResult = stringList.stream()
				.filter(value -> value != null)
				.filter(value -> value.equalsIgnoreCase(query))
				.findFirst();
		if (queryResult.isPresent()) {
			System.out.println("Found " + query + " in list");
		} else {
			System.out.println("Could not find " + query + " in list");
		}
		
		//Example 2 
		//Finding element in list
		final String search = "shooting";
		System.out.println("Finding " + search + " in games");
		queryResult = stringList.stream()
				.filter(value -> value != null)
				.filter(value -> value.equalsIgnoreCase(search))
				.findFirst();
		if (queryResult.isPresent()) {
			System.out.println("Found " + search + " in list");
		} else {
			System.out.println("Could not find " + search + " in list");
		}
	}
}