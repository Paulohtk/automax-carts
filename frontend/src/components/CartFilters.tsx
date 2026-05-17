type CartFiltersProps = {
  userId: string
  startDate: string
  endDate: string
  onUserIdChange: (value: string) => void
  onStartDateChange: (value: string) => void
  onEndDateChange: (value: string) => void
  onClear: () => void
}

export function CartFilters({
  userId,
  startDate,
  endDate,
  onUserIdChange,
  onStartDateChange,
  onEndDateChange,
  onClear,
}: CartFiltersProps) {
  const hasActiveFilters = Boolean(userId || startDate || endDate)

  return (
    <form className="filters-bar" aria-label="Filtros de carrinhos">
      <label>
        <span>User ID</span>
        <input
          inputMode="numeric"
          type="text"
          value={userId}
          onChange={(event) => onUserIdChange(event.target.value.replace(/\D/g, ''))}
          placeholder="Todos"
        />
      </label>

      <label>
        <span>Data inicial</span>
        <input
          type="date"
          value={startDate}
          onChange={(event) => onStartDateChange(event.target.value)}
        />
      </label>

      <label>
        <span>Data final</span>
        <input
          type="date"
          value={endDate}
          onChange={(event) => onEndDateChange(event.target.value)}
        />
      </label>

      <button type="button" className="ghost-button" disabled={!hasActiveFilters} onClick={onClear}>
        Limpar filtros
      </button>
    </form>
  )
}
