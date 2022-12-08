using System;
using System.Collections;

public class Heap<T> where T : IHeapItem<T>     // generic heap itemleri karsilastirabilmek icin Heap<T>'nin Icomparable interface'den turediginden emin oluyoruz (asagidaki interface inheritance bu yuzden var)
{
    T[] items;
    int currentItemCount;

    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];   // generic bir type ile yeni array constructor
    }
    public void Add(T item)
    {
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;     // gecici olarak (T item) array sonuna ekleniyor
        SortUp(item);
        currentItemCount++;
    }
    public T RemoveFirst()
    {
        T firsItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];     // heap diagramdaki son itemi en basa almak
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return firsItem;
    }
    public void UpdateItem(T item) {
        SortUp(item);
    }
    public void Clear() {
        currentItemCount = 0;
    }
    public int Count { 
        get {
            return currentItemCount;
        } 
    }
    public bool Contains(T item) {
        if (item.HeapIndex < currentItemCount){
            return Equals(items[item.HeapIndex], item);
        }
        else{
            return false;
        }
    }
    void SortDown(T item)
    {
        while (true)
        {
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;       // heap diagramda childindex bulma formulu
            int swapIndex = 0;

            if (childIndexLeft<currentItemCount)
            {
                swapIndex = childIndexLeft;

                if (childIndexRight<currentItemCount)
                {
                    if (items[childIndexLeft].CompareTo(items[childIndexRight])<0)
                    {
                        swapIndex = childIndexRight;
                    }
                }
                if (item.CompareTo(items[swapIndex])<0)
                {
                    Swap(item, items[swapIndex]);
                }
                else { 
                    return;
                }
            }
            else{                   // parent'da children yoksa looptan cikmak
                return;
            }

        }
    }
    void SortUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2;     // Heap'de matematiksel olarak parentIndex bulma yontemi
        while (true)
        {
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem)>0)       // higher priority durumunda bu blok returns 1 
            {
                Swap(item, parentItem); 
            }
            else
            {
                break;
            }
            parentIndex = (item.HeapIndex - 1) / 2;         // higher priority oldugu surece parentIndexi tekrar hesaplayip yeni parentle karsilastir
        }
    }
    void Swap(T itemA, T itemB)
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;       // swap oncesi lokal var ile heapIndexi kaydet
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}
public interface IHeapItem<T> : IComparable<T>      // generic heap itemler icin comparable'dan inheritance 
{
    int HeapIndex { get; set; }
}
