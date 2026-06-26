namespace MauiUiComponents;

public static class ObjectExtensions
{
    public static T For<T>(
        this T mainObject,
        int count,
        Action<T, int> action)
    {
        for (int i = 0; i < count; i++)
        {
            action(mainObject, i);
        }
        
        return mainObject;
    }

    public static T Foreach<T, TObj>(
        this T mainObject,
        IEnumerable<TObj> objects,
        Action<T, TObj> action)
    {
        foreach (TObj curObject in objects)
        {
            action(mainObject, curObject);
        }

        return mainObject;
    }
}
