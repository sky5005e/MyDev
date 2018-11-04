
public class program {

	public static void main(String[] args) 
		// TODO Auto-generated method stub
	{
		meschino_sample m = new meschino_sample();
		
		try
		{
			m.Setup();
			m.login();
		}
		catch (Exception e)
		{
			System.out.println(e.getMessage());
		}
}
}