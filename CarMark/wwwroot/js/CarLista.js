document.addEventListener('DOMContentLoaded', () => {

    document.querySelectorAll('input[name="FilterUsed"]').forEach(radio => {
        radio.addEventListener('change', () => {
            document.querySelectorAll('.cond-btn').forEach(btn => btn.classList.remove('on'));
            radio.closest('.cond-btn').classList.add('on');
        });
    });

});
function clearFilters() {
    window.location.href = '/CarLista';
}