(() => {
    const metadata = {
        BRL: { locale: "pt-BR", code: "BRL" },
        USD: { locale: "en-US", code: "USD" },
        EUR: { locale: "de-DE", code: "EUR" },
        GBP: { locale: "en-GB", code: "GBP" }
    };
    const selector = document.getElementById("currency-selector");
    const status = document.getElementById("currency-rate-status");
    const prices = [...document.querySelectorAll("[data-price-amount]")];
    if (!selector || !prices.length) return;

    const savedCurrency = localStorage.getItem("carmark-currency");
    if (savedCurrency && metadata[savedCurrency]) selector.value = savedCurrency;

    const format = (amount, currency) => new Intl.NumberFormat(metadata[currency].locale, {
        style: "currency", currency, maximumFractionDigits: 0
    }).format(amount);

    async function latestRate(from, to) {
        if (from === to) return { rate: 1, date: null };
        const response = await fetch(`https://api.frankfurter.dev/v2/rate/${from}/${to}`);
        if (!response.ok) throw new Error("Exchange rate unavailable");
        return response.json();
    }

    async function renderPrices() {
        const target = selector.value;
        const sourceCurrencies = [...new Set(prices.map(price => price.dataset.priceCurrency || "BRL"))];
        if (status) status.textContent = "Updating live exchange rates…";

        try {
            const results = await Promise.all(sourceCurrencies.map(source => latestRate(source, target)));
            const rates = new Map(sourceCurrencies.map((source, index) => [source, results[index]]));
            prices.forEach(element => {
                const amount = Number(element.dataset.priceAmount);
                const source = element.dataset.priceCurrency || "BRL";
                if (Number.isFinite(amount)) element.textContent = format(amount * rates.get(source).rate, target);
            });
            const date = results.find(result => result.date)?.date;
            if (status) status.textContent = date ? `Live rates · ${date}` : "Original currency";
        } catch {
            prices.forEach(element => {
                const amount = Number(element.dataset.priceAmount);
                const source = element.dataset.priceCurrency || "BRL";
                if (Number.isFinite(amount)) element.textContent = format(amount, source);
            });
            if (status) status.textContent = "Live rates unavailable — showing original prices";
        }
    }

    selector.addEventListener("change", () => {
        localStorage.setItem("carmark-currency", selector.value);
        renderPrices();
    });
    renderPrices();
})();
