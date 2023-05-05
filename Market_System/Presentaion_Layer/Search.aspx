<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="YourProjectNamespace.Search" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<!DOCTYPE html>
<html>
<head>
    <title>Search and Categories</title>
    <style>
        /* CSS styles for search and categories */
        .container {
            display: flex;
            flex-direction: row;
            justify-content: space-between;
            align-items: center;
            margin: 20px;
        }

        .search-bar {
            width: 60%;
            padding: 10px;
            border: 2px solid #ccc;
            border-radius: 5px;
            font-size: 16px;
            background-color: #f8f8f8;
        }

        .category-select {
            width: 30%;
            padding: 10px;
            border: 2px solid #ccc;
            border-radius: 5px;
            font-size: 16px;
            background-color: #f8f8f8;
        }

        .item {
            margin: 10px;
            padding: 10px;
            border: 2px solid #ccc;
            border-radius: 5px;
            background-color: #f8f8f8;
            display: flex;
            align-items: center;
        }

            .item img {
                width: 50px;
                height: 50px;
                margin-right: 10px;
            }
    </style>
    <link href="Site.Master" rel="import" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <!-- Search bar -->
            <asp:TextBox ID="searchBar" CssClass="search-bar" runat="server" placeholder="Search..." OnTextChanged="searchItems" AutoPostBack="true"></asp:TextBox>

            <!-- Category select -->
            <asp:DropDownList ID="categorySelect" CssClass="category-select" runat="server" OnSelectedIndexChanged="filterItems">
                <asp:ListItem Text="All Categories" Value=""></asp:ListItem>
                <asp:ListItem Text="Men" Value="category1"></asp:ListItem>
                <asp:ListItem Text="Women" Value="category2"></asp:ListItem>
                <asp:ListItem Text="Children" Value="category3"></asp:ListItem>
            </asp:DropDownList>
        </div>

        <!-- List of items -->
        <div id="Div1" runat="server">
            <div class="item category1">
                <img src="https://litb-cgis.rightinthebox.com/images/640x640/202211/bps/product/inc/ihkbey1668222138935.jpg" alt="Pants">
                pants
            </div>
            <div class="item category2">
                <img src="https://images.asos-media.com/products/asos-design-ultimate-t-shirt-with-crew-neck-in-cotton-blend-in-black-black/201351926-1-black?$n_480w$&wid=476&fit=constrain" alt="Tshirt">
                Tshirts
            </div>
            <div class="item category3">
                <img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSPOJPIfzHr7tPsli9syWCX1NJj5kr7WqvQ7g&usqp=CAU" alt="Pants">
                pants
        </div>
        <div class="item category1">
            <img src="https://www.next.co.il/nxtcms/resource/image/5487892/portrait_ratio4x5/320/400/33f0eec5e67c6fa86a8ad0f39181d3b6/7F5B416972E2D3FDFDF7ED6405B5D428/shirts.jpg" alt="Shirt">
            shirts
        </div>
        <div class="item category2">
            <img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTrY--pqDNzx0jpa3YNJZeGy5FuQpARzP3YdA&usqp=CAU" alt="Dress">
            dresses
        </div>
        <div class="item category3">
            <img src="https://xcdn.next.co.uk/COMMON/Items/Default/Default/ItemImages/AltItemShot/315x472/C84056s.jpg" alt="Shoes">
            shoes
        </div>
    </div>

    <script>
        // JavaScript code for search and categories
        const searchInput = document.querySelector('.search-bar');
        const categorySelect = document.querySelector('.category-select');
        const itemList = document.querySelector('#item-list');
        const items = itemList.querySelectorAll('.item');

        // Add event listeners to search bar and category select
        searchInput.addEventListener('keyup', searchItems);
        categorySelect.addEventListener('change', filterItems);

        // Search function

        function searchItems() {
            const searchValue = searchInput.value.toLowerCase();

            for (let i = 0; i < items.length; i++) {
                const itemText = items[i].textContent.toLowerCase();

                if (itemText.includes(searchValue)) {
                    items[i].style.display = '';
                } else {
                    items[i].style.display = 'none';
                }
            }
        }

        // Filter function
        function filterItems() {
            const categoryValue = categorySelect.value;

            for (let i = 0; i < items.length; i++) {
                const itemCategory = items[i].classList.contains(categoryValue);

                if (categoryValue === '' || itemCategory) {
                    items[i].style.display = '';
                } else items[i].style.display = 'none';
            }
        }

    </script>
</body>
</html>
    </asp:Content>
