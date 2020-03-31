/* 
 * James Hadley 
 * T7124647
 * Group: The Spanish Inquisition
 * AI for Games Engines (GAV3005-N-FJ1-2019)
 * Teesside University
 */

using System;

// Class Heap implements the interface IHeapItem using where T
public class Heap<T> where T : IHeapItem<T>
{
    T[] items;
    int currentItemCount;

    public Heap(int maxHeapSize)
    {
        // Items equals a new array T with the size of maxHeapSize
        items = new T[maxHeapSize];
    }

    // Method used to add items to the heap
    public void Add(T item)
    {
        // Set the HeapIndex to equal currentItemCount
        item.HeapIndex = currentItemCount;
        // Add items to the end of currentIemCount array by setting it equal to item
        items[currentItemCount] = item;
        // Pass in the new item
        SortUp(item);
        // Increment the currentItemCount by 1
        currentItemCount++;
    }

    // Method used to remove the first item from the heap
    public T RemoveFirstItem()
    {
        // Save the first item, set it equal to items[0]
        T firstItem = items[0];
        // Decrement currentItemCount to have 1 less item on the heap
        currentItemCount--;

        // Take item at the end of the heap and put into the first place of the heap
        items[0] = items[currentItemCount];
        // Set heap index of the item to 0
        items[0].HeapIndex = 0;

        // Pass down item, to sort heap
        SortDown(items[0]);

        return firstItem;
    }

    // Method used to update an item, this method is used in case the priority of an item is changed
    public void UpdateItem(T item)
    {
        // Increase priority for the item
        SortUp(item);
    }

    // Public accessor to get the number of items currently in the heap
    public int Count
    {
        get
        {
            return currentItemCount;
        }
    }

    // Method used to check if the heap contains a specific item
    public bool Contains(T item)
    {
        // Equals method used to check if two items are equal
        // Check if the items array is equal to the item that is being passed in from the heap index
        // If it is then return true, if it is not equal to then return false
        return Equals(items[item.HeapIndex], item);
    }

    // Method used to sort the order down the heap
    void SortDown(T item)
    {
        // Loop through the indices of the item to the children
        while (true)
        {
            // Declare left and right index for the child
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            // Set swap index to equal 0
            int swapIndex = 0;

            // Check if item has at least one child on the left of heap
            // If the currentItemCount is less than the childIndex on the left of heap
            if (childIndexLeft < currentItemCount)
            {
                // Swap index in the heap to equal the child index left of the heap
                swapIndex = childIndexLeft;

                // Check if item has an item on the right second child
                // If the child index right is less than currentItemCount then:
                if (childIndexRight < currentItemCount)
                {
                    // Check which of the two children has got a higher priority using the CompareTo function
                    // Swap Index is currently defaulted to left child index, so if the left child index has
                    // a lower priority then we set the statement to be less than 0
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                    {
                        // Set swap index to childIndexRight
                        swapIndex = childIndexRight;
                    }
                }

                // Check if the parent has a lower priority in comparison to the highest priority child
                // If the parent has a lower priority to the child then swap them
                // If the parent item index is less than 0 then swap with child index 
                if (item.CompareTo(items[swapIndex]) < 0)
                {
                    // Swap parent item with child item
                    Swap(item, items[swapIndex]);
                }
                // If the parent has a higher priority than both children then it is in the correct position 
                else
                {
                    // Return to exist the loop
                    return;
                }
            }
            // If the parent doesn't have children then it is in the correct position
            else
            {
                // Return to exist the loop
                return;
            }
        }
    }

    // Method used to sort the order up the heap
    void SortUp(T item)
    {
        // Calculate parent index
        int parentIndex = (item.HeapIndex - 1) / 2;

        // Loop through parent item when true
        while (true)
        {
            // Set parent item to equal parentIndex
            T parentItem = items[parentIndex];

            // Compare parentItem to see whether is it higher than 0
            // If the parentItem has a higher priority it then returns 1
            // If it's got the same priority it then returns 0
            // If it's got a lower priority it then returns -1
            // If item has got a higher priority than the parentItem, it has a lower F cost, then it is swapped with the parentItem 
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else
            {
                break;
            }

            // Keep recalculating the parentIndex and comparing the item to the parent
            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    // Swap heap method
    void Swap(T itemA, T itemB)
    {
        // Swap itemA and itemB in the index array
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;

        // Swap HeapIndex value
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }

}

// This is a generic interface where T is the type
// for a data type that will be provided by the implementing class.
// IComparable<T> interface is also implemented, used to define comparison
// of the value type T, used for the ordering and sortings of the instances in the heap.
public interface IHeapItem<T> : IComparable<T>
{
    // Int heap index that can both get and set the int
    int HeapIndex
    {
        get;
        set;
    }
}