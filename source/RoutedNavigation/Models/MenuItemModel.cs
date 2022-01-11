// This software may be modified and distributed under the terms of the MIT license.  
// See the LICENSE file in the project root for details.

using System;
using System.Collections.Generic;

namespace RoutedNavigation.Models
{

    public class MenuItemModel
    {
        public string Title { get; set; }

        public string Url { get; set; }

        public Type ViewModel { get; set; }

        public List<MenuItemModel> Children { get; set; }
            = new List<MenuItemModel>();
    }
}
