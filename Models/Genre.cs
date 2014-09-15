using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WecareMVC.Models
{
    public class Genre
    {
        [Key]
        [DisplayName("分類ID")]
        public int GenreId { get; set; }
        [DisplayName("分類名稱")]
        public string Name { get; set; }
        [DisplayName("分類描述")]
        public string Description { get; set; }
        [DisplayName("分類商品")]
        public List<Album> Albums { get; set; }
        //public virtual List<Album> Albums { get; set; }
    



        public IEnumerable<Album> GetTopAlbum(int count)
        {
            MusicStoreEntities db = new MusicStoreEntities();
            var Albums = db.Albums.Where(a => a.GenreId == GenreId).
                OrderByDescending(a => a.OrderDetails.Sum(o=>o.Quantity))
                .Take(count)
                .ToList();
            return Albums;
        }
    }

    //should add using WecareMVC.Models in Razor view
    public static class GenreExtension
    {
        public static IEnumerable<Album> GetTopAlbum(this  WecareMVC.Models.Genre genre, int count)
        {
            MusicStoreEntities db = new MusicStoreEntities();
            var Albums = db.Albums.Where(a=>a.GenreId==genre.GenreId).
                OrderByDescending(a => a.OrderDetails.Sum(o => o.Quantity))
                .Take(count)
                .ToList();
            return Albums;
        }
    }  
}
       // private Album[] _Album;

//        public Genre() : this(new Album[5])

//        public Genre(Album[] pArray)
//    {
//        _Album = new Album[pArray.Length];

//        for (int i = 0; i < pArray.Length; i++)
//        {
//            _Album[i] = pArray[i];
//        }
//    }

//        IEnumerator IEnumerable.GetEnumerator()
//        {
//            return (IEnumerator)GetEnumerator();
//        }

//        public PeopleEnum GetEnumerator()
//        {
//            return new PeopleEnum(_Album);
//        }
//    }

//    public class PeopleEnum : IEnumerator
//    {
//        public Album[] _people;

//        // Enumerators are positioned before the first element
//        // until the first MoveNext() call.
//        int position = -1;

//        public PeopleEnum(Album[] list)
//        {
//            _people = list;
//        }

//        public bool MoveNext()
//        {
//            position++;
//            return (position < _people.Length);
//        }

//        public void Reset()
//        {
//            position = -1;
//        }

//        object IEnumerator.Current
//        {
//            get
//            {
//                return Current;
//            }
//        }

//        public Album Current
//        {
//            get
//            {
//                try
//                {
//                    return _people[position];
//                }
//                catch (IndexOutOfRangeException)
//                {
//                    throw new InvalidOperationException();
//                }
//            }
//        }
//    }
//}