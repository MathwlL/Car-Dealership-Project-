document.addEventListener("DOMContentLoaded", function () {

    const buttons = document.querySelectorAll(".filter-buttons button");
    const cards = document.querySelectorAll(".card");

    buttons.forEach(button => {
        button.addEventListener("click", () => {

            buttons.forEach(btn => btn.classList.remove("active"));

            button.classList.add("active");

            const filter = button.getAttribute("data-filter");

            cards.forEach(card => {
                if (filter === "all" || card.classList.contains(filter)) {
                    card.style.display = "block";
                } else {
                    card.style.display = "none";
                }
            });

        });
    });

});