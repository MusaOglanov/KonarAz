﻿namespace Konar.az.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<BlogTag>? BlogTags { get; set; }
        public List<ProductTag>? ProductTags { get; set; }
    }
}
