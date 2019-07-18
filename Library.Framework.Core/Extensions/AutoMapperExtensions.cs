using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Library.Framework.Core.Model;

namespace Acb.Plugin.FanCheHui.Contract.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class AutoMapperExtensions
    {
        private static IMapper Create(Type sourceType, Type destinationType, TypeMap[] maps = null)
        {
            var cfg = new MapperConfiguration(config =>
            {
                if (maps != null && maps.Any())
                {
                    foreach (var map in maps)
                    {
                        config.CreateMap(map.SourceType, map.DestinationType);
                    }
                }

                config.CreateMap(sourceType, destinationType);
                config.ValidateInlineMaps = false;
            });
            return cfg.CreateMapper();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T MapTo<T>(this object source)
        {
            if (source == null)
                return default(T);
            Type sourceType = source.GetType(), deestinationType = typeof(T);
            if (source is IEnumerable listSource)
            {
                foreach (var item in listSource)
                {
                    sourceType = item.GetType();
                    break;
                }
            }
            return Create(sourceType, deestinationType).Map<T>(source);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<T> MapTo<T>(this IEnumerable source)
        {
            return ((object)source).MapTo<List<T>>();
        }


        /// <summary> 映射成分页类型 </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="pagedList"></param>
        /// <returns></returns>
        public static PagedList<T> MapPagedList<T, TSource>(this PagedList<TSource> pagedList)
        {
            if (pagedList == null)
                return new PagedList<T>();
            var list = pagedList.DataList.MapTo<T>();
            return new PagedList<T>(list, pagedList.Index, pagedList.Size, pagedList.Total);
        }
    }
}
